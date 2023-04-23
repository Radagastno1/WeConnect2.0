using Dapper;
using MySqlConnector;
using CORE.ENTITIES;
namespace INFRASTRUCTURE.MYSQL;
public class UsersDB 
{
     public User GetUserById(int? id)
    {
        User foundUser = new();
        try
        {
            string query = 
                "SELECT u.id, u.first_name as 'FirstName', u.last_name as 'LastName', " +
               "DATE_FORMAT(u.birth_date, '%Y-%m-%d') as 'BirthDate', u.gender, " +
               "u.about_me as 'AboutMe' " +
               "FROM users u " +
               "WHERE u.role_id = 5 AND u.is_deleted = false AND id = @id;";
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            foundUser = con.QuerySingle<User>(query, new { @id = id});
            return foundUser;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    public List<User> GetSearches(string name)
    {
        List<User> foundUsers = new();
        string query = "SELECT u.id as 'ID' FROM users u " +
        $"WHERE u.first_name LIKE '%{name}%' OR u.last_name LIKE '%{name}%' AND u.is_active = true;";
        //SÖKNING FÅR SKE ATT FÖRST KÖRS DENNA - OCH SEDAN HÄMTAS VAR OCH EN AV DE HITTADE USERS GENOM GETBYID
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        foundUsers = con.Query<User>(query, new { @name = name }).ToList();
        return foundUsers;
    }
    public User GetOne(int id, User user)
    {
        User foundUser = new();
        try
        {
            string query = "SELECT u.id, u.first_name as 'FirstName', u.last_name as 'LastName', " +
               "DATE_FORMAT(u.birth_date, '%Y-%m-%d') as 'BirthDate', u.gender, " +
               "u.about_me as 'AboutMe' " +
               "FROM users u " +
               "WHERE u.role_id = 5 AND u.is_deleted = false " +
               "AND u.id = @id " +
               "AND u.id not in (select blocked_user_id from users_blocked where users_id = @userId) " +
               "AND u.id not in (select users_id FROM users_blocked where blocked_user_id = @userId);";
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            foundUser = con.QuerySingle<User>(query, new { @id = id, @userId = user.ID });
            return foundUser;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    public List<User> GetInactive()
    {
        List<User> users = new();
        string query = "SELECT u.id as 'Id', DATE_ADD(u.date_inactive, interval 30 day) " +
        "as deletingdate FROM users u  WHERE DATE_ADD(u.date_inactive, interval 30 day) < CURRENT_DATE() " +
        "AND is_deleted = false;";
        try
        {
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;Allow User Variables=true;");
            users = con.Query<User>(query).ToList();
            return users;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    public void UpdateToDeleted(User user)
    {
        string query = "START TRANSACTION; " +
        "UPDATE users SET is_deleted = true WHERE id = @Id; " +
        "UPDATE messages SET is_visible = false WHERE sender_id = @Id;" +
        "COMMIT;";
        using (MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;Allow User Variables=true;"))
            con.ExecuteScalar<int>(query, param: user);
    }
}