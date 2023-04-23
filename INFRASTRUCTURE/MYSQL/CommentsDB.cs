using CORE.ENTITIES;
using Dapper;
using MySqlConnector;

namespace WeConnect.Data;

public class CommentsDB
{
    public List<Comment> GetById(int postId, User user)
    {
        List<Comment> commentsOnPost = new();
        string query =
            "SELECT p.id, p.content, p.date_created AS 'DateCreated', p.users_id AS 'UserId', "
            + "p.on_post_id AS 'OnPostId', u.first_name AS 'FirstName', u.last_name AS 'LastName' FROM posts p "
            + "INNER JOIN users u ON p.users_id = u.id "
            + "WHERE p.post_type = 'Comment' AND p.is_visible = TRUE AND p.on_post_id = @postId;";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        commentsOnPost = con.Query<Comment>(query, new { @postId = postId }).ToList();
        return commentsOnPost;
    }
}
