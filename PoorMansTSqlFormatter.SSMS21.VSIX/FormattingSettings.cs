using PoorMansTSqlFormatterLib.Formatters;
using System;
using System.IO;
using System.Text;

namespace PoorMansTSqlFormatter.SSMS21.VSIX
{
    /// <summary>
    /// Lädt und speichert die Formatierungsoptionen in einer Config-Datei unter
    /// %AppData%\PoorMansTSqlFormatter\formatting-options.txt. Das Dateiformat ist
    /// die eigene Key-Wert-Serialisierung der Bibliothek (z.B. "UppercaseKeywords=True").
    /// Fehler beim Lesen/Schreiben werden geschluckt; bei beschädigter Datei gelten die Defaults.
    /// </summary>
    internal static class FormattingSettings
    {
        private static readonly object SyncLock = new object();
        private static TSqlStandardFormatterOptions _current;

        /// <summary>Pfad zur Config-Datei (dauerhaft, unabhängig vom Extensions-Ordner).</summary>
        public static string ConfigFilePath
        {
            get
            {
                string dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "PoorMansTSqlFormatter");
                return Path.Combine(dir, "formatting-options.txt");
            }
        }

        /// <summary>Aktive Optionen (bei Bedarf aus der Config geladen).</summary>
        public static TSqlStandardFormatterOptions Current
        {
            get
            {
                lock (SyncLock)
                {
                    if (_current == null)
                        _current = Load();
                    return _current;
                }
            }
        }

        /// <summary>Speichert die übergebenen Optionen und macht sie ab sofort aktiv.</summary>
        public static void Save(TSqlStandardFormatterOptions options)
        {
            lock (SyncLock)
            {
                _current = options;
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ConfigFilePath));
                    File.WriteAllText(ConfigFilePath, options.ToSerializedString(), new UTF8Encoding(false));
                }
                catch
                {
                    // Speichern darf die Nutzung nie blockieren.
                }
            }
        }

        /// <summary>Verwirft den Cache und lädt neu (vor allem nach einem externen Edit der Datei).</summary>
        public static void Reload()
        {
            lock (SyncLock)
            {
                _current = Load();
            }
        }

        private static TSqlStandardFormatterOptions Load()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string serialized = File.ReadAllText(ConfigFilePath);
                    if (!string.IsNullOrWhiteSpace(serialized))
                        return new TSqlStandardFormatterOptions(serialized);
                }
            }
            catch
            {
                // Beschädigte Config -> Defaults.
            }
            return new TSqlStandardFormatterOptions();
        }
    }
}
