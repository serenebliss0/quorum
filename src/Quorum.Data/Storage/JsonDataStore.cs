using System.Text.Json;
using System.Text.Json.Serialization;
using Quorum.Data.Models;

namespace Quorum.Data.Storage;

public class JsonDataStore : IDataStore
{
    private readonly string _dataDirectory;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public JsonDataStore(string? dataDirectory = null)
    {
        _dataDirectory = dataDirectory ??
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "Quorum");

        Directory.CreateDirectory(_dataDirectory);
    }

    private string GetPath(string fileName)
    {
        return Path.Combine(_dataDirectory, fileName);
    }

    private T Load<T>(string fileName, T fallback)
    {
        string path = GetPath(fileName);

        if (!File.Exists(path))
            return fallback;

        string json = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(json))
            return fallback;

        return JsonSerializer.Deserialize<T>(json, _options)
               ?? fallback;
    }

    private void Save<T>(string fileName, T data)
    {
        string path = GetPath(fileName);

        string json = JsonSerializer.Serialize(data, _options);

        File.WriteAllText(path, json);
    }

    public List<User> LoadUsers()
    {
        string path = GetPath("users.json");

        if (!File.Exists(path))
            return [];

        string json = File.ReadAllText(path);

        using JsonDocument document = JsonDocument.Parse(json);

        List<User> users = [];

        foreach (JsonElement element in document.RootElement.EnumerateArray())
        {
            UserRole role = element.GetProperty("Role")
                .Deserialize<UserRole>(_options);

            if (role == UserRole.Student)
            {
                Student? student =
                    element.Deserialize<Student>(_options);

                if (student is not null)
                    users.Add(student);
            }
            else
            {
                User? user = element.Deserialize<User>(_options);

                if (user is not null)
                    users.Add(user);
            }
        }

        return users;
    }

    public void SaveUsers(List<User> users)
    {
        Save("users.json", users);
    }

    public List<Course> LoadCourses()
    {
        return Load("courses.json", new List<Course>());
    }

    public void SaveCourses(List<Course> courses)
    {
        Save("courses.json", courses);
    }

    public List<Registration> LoadRegistrations()
    {
        return Load("registrations.json", new List<Registration>());
    }

    public void SaveRegistrations(List<Registration> registrations)
    {
        Save("registrations.json", registrations);
    }

    public AcademicPolicy LoadPolicy()
    {
        return Load("policy.json", new AcademicPolicy());
    }

    public void SavePolicy(AcademicPolicy policy)
    {
        Save("policy.json", policy);
    }
}