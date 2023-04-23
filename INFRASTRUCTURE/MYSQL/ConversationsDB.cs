using CORE.ENTITIES;
using Dapper;
using MySqlConnector;

namespace WeConnect.Data;

public class ConversationDB
{
    public List<Conversation> GetConversationsOfSpecificParticipants(int amountOfUsers, string sql)
    {
        List<Conversation> conversations = new();
        string query =
            $"SELECT uc.conversations_id AS 'ID', "
            + "GROUP_CONCAT(uc.users_id) AS User_List "
            + "FROM users_conversations uc "
            + $"WHERE  uc.users_id IN ({sql})"
            + "GROUP BY uc.conversations_id "
            + "HAVING COUNT(uc.users_id) = @amountOfUsers;";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        conversations = con.Query<Conversation>(query, new { @amountOfUsers = amountOfUsers })
            .ToList();
        return conversations;
    }

    public ConversationResult GetConversationIdAndParticipantNames(int conversationId)
    {
        List<User> users = new();
        ConversationResult result = new();
        string query =
            $" SELECT c.id as 'ID', GROUP_CONCAT(u.first_name) AS ParticipantsNames "
            + "FROM conversations c "
            + "INNER JOIN users_conversations uc "
            + "ON uc.conversations_id = c.id "
            + "INNER JOIN users u "
            + "ON u.id = uc.users_id "
            + "WHERE c.id = @id AND u.is_active = true;";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        result.Conversation = con.QuerySingle<Conversation>(query, new { @id = conversationId });
        if (result.Conversation != null)
        {
            result.ConversationExists = true;
        }
        return result;
    }

    public List<Conversation> GetById(int id, User user)
    {
        // List<Conversation> conversations = new();
        // string query = $"SELECT uc.conversation_id as 'ID' FROM users_conversations uc WHERE uc.users_id = @id;";
        // using (MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"))
        // {
        //     conversations = con.Query<Conversation>(query).ToList();
        // }
        // return conversations;
        throw new NotImplementedException();
    }

    public Conversation GetDialogueId(int userId, int id)
    {
        Conversation dialogue = new();
        string query =
            "select uc.conversations_id as 'Id'"
            + "from users_conversations uc "
            + "group by uc.conversations_id "
            + "having sum(uc.users_id in (@userId, @id)) = count(*);";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        dialogue = con.QuerySingle<Conversation>(query, new { @userId = userId, @id = id });
        return dialogue;
    }

    public List<Conversation> GetUnreadConversations(User user)
    {
        string query =
            "SELECT c.id as ID, GROUP_CONCAT(u.first_name) AS ParticipantsNames "
            + "FROM conversations c "
            + "INNER JOIN users_conversations uc ON uc.conversations_id = c.id "
            + "INNER JOIN users u ON u.id = uc.users_id "
            + "WHERE u.is_active = 1 AND uc.is_read = false AND c.id IN ( "
            + "SELECT conversations_id "
            + "FROM users_conversations "
            + "WHERE users_id = @userId "
            + "AND is_read = false) "
            + "GROUP BY c.id; ";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        List<Conversation> conversations = con.Query<Conversation>(query, new { @userId = user.ID })
            .ToList();
        if (conversations != null)
        {
            return conversations;
        }
        return null;
    }
    public async Task UpdateConversationToReadAsync(int conversationId, int userId)
    {   
        string query = "UPDATE users_conversations " + 
        "SET is_read = true " + 
        "WHERE users_id = @userId AND conversations_id = @conversationId;";
         using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        await con.ExecuteScalarAsync(query, new{@userId = userId, @conversationId = conversationId});
    }
}
