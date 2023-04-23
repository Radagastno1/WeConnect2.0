using CORE.ENTITIES;
using Dapper;
using MySqlConnector;

namespace WeConnect.Data;

public class PostsDB
{
    public List<Post> GetById(int userId, User user)
    {
        List<Post> posts = new();
        string query =
            $"SELECT p.id as 'Id', p.content as 'Content', p.date_created as 'DateCreated', u.first_name as 'FirstName', u.last_name as 'LastName', p.users_id as 'UserId' "
            + $"FROM posts p INNER JOIN users u ON p.users_id = u.id WHERE p.post_type = 'Post' AND p.is_visible = TRUE AND p.is_deleted = FALSE AND p.users_id = @userId "
            + "AND p.users_id not in "
            + "(select blocked_user_id from users_blocked where users_id = @myId) "
            + "AND p.users_id not in "
            + "(select users_id from users_blocked where blocked_user_id = @myId);";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        posts = con.Query<Post>(query, new { @userId = userId, @myId = user.ID }).ToList();
        if (posts.Count > 0)
        {
            return posts;
        }
        else
        {
            return null;
        }
    }

    public Post GetOne(int postId, User user) //byt namn på metoden
    {
        Post post = new();
        string query =
            $"SELECT p.id as 'Id', p.content as 'Content', p.date_created as 'DateCreated', u.first_name as 'FirstName', u.last_name as 'LastName', p.users_id as 'UserId' "
            + $"FROM posts p INNER JOIN users u ON p.users_id = u.id WHERE p.post_type = 'Post' AND p.is_visible = TRUE AND p.is_deleted = FALSE AND p.id = @postId;";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        post = con.QuerySingle<Post>(query, new { @postId = postId });
        if (post != null)
        {
            return post;
        }
        else
        {
            return null;
        }
    }
}
