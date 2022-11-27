using System.IO;
using System.Text;

namespace WpfDemoApp;

internal sealed class Player
{
    public required string Name { get; set; }
    public required Enums.Job Job { get; set; }
    public required int Mp { get; set; }

    public bool Valid
    {
        get
        {
            // 1~4
            if (Name.Length is < 1 or > 4)
                return false;

            if (Job is Enums.Job.NotImplemented)
                return false;

            if (RequireMp())
            {
                // 1~999
                if (Mp is < 1 or > 999)
                    return false;
            }
            return true;
        }
    }

    public static bool RequireMp(Enums.Job job) => job switch
    {
        Enums.Job.Wizard => true,
        _ => false
    };

    bool RequireMp() => RequireMp(Job);

    public string ToText()
    {
        StringBuilder sb = new();
        sb.AppendLine(Name);
        sb.AppendLine(((int)Job).ToString());
        if (RequireMp())
            sb.AppendLine(Mp.ToString());
        return sb.ToString();
    }

    public string ToBase64()
    {
        string text = ToText();
        ReadOnlySpan<byte> span = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(span);
    }

    public void Save(string filePath)
    {
        string base64String = ToBase64();
        File.WriteAllText(filePath, base64String, new UTF8Encoding(false)); // without BOM
    }
}
