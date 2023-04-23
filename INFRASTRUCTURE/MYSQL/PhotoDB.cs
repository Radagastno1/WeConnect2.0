using CORE.ENTITIES;
using Dapper;
using MySqlConnector;

namespace WeConnect.Data;

public class PhotoDB
{
    public string UpdateProfilePhoto(User user, string image_url)
    {
        string query = "CALL add_profile_photo(@image_url, @user_id);";
          try
        {
            using MySqlConnection con =
                new($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            var imageUrl = con.QuerySingle<string>(query, new { @user_id = user.ID, @image_url = image_url});
            return imageUrl;
        }
        catch { 
            return null;
        }
    }
    public async Task<Photo> GetProfilePhotoAsync(User user)
    {
        string query =
            "SELECT p.id, p.image_url AS 'ImageURL' FROM photos p " +
            "INNER JOIN photo_album pa " + 
            "ON pa.id = p.photo_album_id " + 
            "INNER JOIN users u " + 
            "ON u.id = pa.users_id " + 
            "WHERE u.id = @id AND u.profile_photo_id = p.id";
        try
        {
            using MySqlConnection con =
                new($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            var photo = await con.QuerySingleAsync<Photo>(query, new { @id = user.ID });
            return photo;
        }
        catch { 
            return null;
        }
    }
}
