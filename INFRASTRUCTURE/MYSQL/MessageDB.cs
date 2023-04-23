using CORE.ENTITIES;
using Dapper;
using MySqlConnector;
namespace WeConnect.Data;
public class MessagesDB
{
    public List<Message> GetById(int conversationId, User user) ///IDATA
    {
        List<Message> messages = new();
        string query = "SELECT m.content as 'Content', concat(u.first_name, ' ', u.last_name) as 'Sender', sender_id as 'SenderId', m.conversations_id as 'ConversationId' " +
       "FROM messages m INNER JOIN conversations c ON m.conversations_id = c.id  " +
       "INNER JOIN users_conversations uc ON c.id = uc.conversations_id " +
       "INNER JOIN users u ON u.id = m.sender_id " +
       "WHERE c.id = @conversationId AND m.is_visible = true GROUP BY m.id " +
       "ORDER BY m.date_created ASC;";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        messages = con.Query<Message>(query, new { @conversationId = conversationId }).ToList();
        return messages;
    } 
}