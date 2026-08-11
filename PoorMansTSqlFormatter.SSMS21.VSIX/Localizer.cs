using System.Collections.Generic;
using System.Globalization;

namespace PoorMansTSqlFormatter.SSMS21.VSIX
{
    /// <summary>
    /// Zentrale Laufzeit-Lokalisierung: deutsches SSMS bekommt deutsche Texte, alle
    /// anderen Systeme englische. Die .vsct-Defaults sind Englisch; auf deutschen
    /// Systemen werden Toolbar-/Menue-Buttons zur Laufzeit per CommandBars
    /// umbeschriftet (siehe <see cref="FormatSqlCommand.ApplyLocalizedCaptions"/>).
    /// </summary>
    internal static class Localizer
    {
        /// <summary>True, wenn das SSMS-System deutsch ist (de-DE, de-AT, de-CH, ...).</summary>
        public static bool IsGerman =>
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "de";

        /// <summary>Waehlt je nach Systemsprache den deutschen oder englischen Text.</summary>
        public static string T(string german, string english) => IsGerman ? german : english;

        /// <summary>
        /// Mapping der .vsct-Buttontexte (englische Defaults) auf die deutschen
        /// Bezeichnungen. Wird bei deutschem SSMS per CommandBars-Caption angewendet.
        /// </summary>
        public static readonly Dictionary<string, string> GermanCommandCaptions =
            new Dictionary<string, string>
            {
                { "Format Selection", "Auswahl formatieren" },
                { "Format Whole Document", "Ganzes Dokument formatieren" },
                { "Formatting Settings…", "Formatierungseinstellungen…" },
            };

        // --- MessageBox-Texte ---

        /// <summary>Keine Auswahl markiert (Info-Dialog).</summary>
        public static string NoSelectionMessage =>
            T(
                "Es ist keine Textauswahl markiert.\r\n\r\n" +
                "Für das komplette Skript: Menüpunkt „Ganzes Dokument formatieren“.",
                "No text is selected.\r\n\r\n" +
                "For the whole script: use the menu item \"Format Whole Document\".");

        /// <summary>Prefix der Fehler-MessageBox (Formatieren fehlgeschlagen).</summary>
        public static string FormatFailedPrefix =>
            T("Formatieren fehlgeschlagen: ", "Formatting failed: ");

        /// <summary>Warnung bei nicht vollständig parsebarem Text (Ja/Nein-Dialog).</summary>
        public static string ParseWarningMessage =>
            T(
                "Die Auswahl konnte nicht vollständig geparst werden.\r\n\r\n" +
                "Tipp: Die vollständige Prozedur markieren (Strg+A oder von ALTER PROCEDURE bis END).\r\n\r\n" +
                "Bei „Ja“ kann das Ergebnis unbrauchbar werden (Treppen-Einrückung) und ersetzt dein SQL.",
                "The selection could not be parsed completely.\r\n\r\n" +
                "Tip: Select the whole procedure (Ctrl+A or from ALTER PROCEDURE to END).\r\n\r\n" +
                "If you choose \"Yes\", the result may be unusable (staircase indentation) and it will replace your SQL.");

        // --- Einstellungs-Dialog ---

        public static string SettingsTitle =>
            T("Poor Man's T-SQL Formatter – Einstellungen", "Poor Man's T-SQL Formatter – Settings");

        public static string[] IndentChoices =>
            new[] { T("Tabulator", "Tab"), T("2 Leerzeichen", "2 spaces"), T("4 Leerzeichen", "4 spaces") };

        public static string IndentGroup =>
            T("Einrückung", "Indentation");

        public static string IndentLabel =>
            T("Einrückung:", "Indentation:");

        public static string SpacesPerTabLabel =>
            T("SpacesPerTab (Breite je Tab):", "SpacesPerTab (width per tab):");

        public static string MaxLineWidthLabel =>
            T("MaxLineWidth (Umbruchgrenze):", "MaxLineWidth (wrap limit):");

        public static string BlankLinesGroup =>
            T("Leerzeilen", "Blank lines");

        public static string StatementBreaksLabel =>
            T("Zwischen Statements (NewStatementLineBreaks):", "Between statements (NewStatementLineBreaks):");

        public static string ClauseBreaksLabel =>
            T("Zwischen Klauseln (NewClauseLineBreaks):", "Between clauses (NewClauseLineBreaks):");

        public static string ExpandListsGroup =>
            T("Listen aufklappen", "Expand lists");

        public static string CommaGroup =>
            T("Kommas", "Commas");

        public static string KeywordGroup =>
            T("Keywords", "Keywords");

        public static string ExpandCommaListsCheck =>
            T("Komma-Listen auf mehrere Zeilen\r\n(ExpandCommaLists)", "Comma lists on multiple lines\r\n(ExpandCommaLists)");

        public static string ExpandBooleanExpressionsCheck =>
            T("AND/OR auf eigene Zeilen\r\n(ExpandBooleanExpressions)", "AND/OR on separate lines\r\n(ExpandBooleanExpressions)");

        public static string ExpandCaseStatementsCheck =>
            T("CASE/WHEN/THEN untereinander\r\n(ExpandCaseStatements)", "CASE/WHEN/THEN stacked\r\n(ExpandCaseStatements)");

        public static string ExpandInListsCheck =>
            T("IN-Listen untereinander\r\n(ExpandInLists)", "IN lists stacked\r\n(ExpandInLists)");

        public static string ExpandBetweenConditionsCheck =>
            T("BETWEEN auf eigene Zeilen\r\n(ExpandBetweenConditions)", "BETWEEN on separate lines\r\n(ExpandBetweenConditions)");

        public static string BreakJoinOnSectionsCheck =>
            T("JOIN…ON auf eigene Zeilen\r\n(BreakJoinOnSections)", "JOIN…ON on separate lines\r\n(BreakJoinOnSections)");

        public static string TrailingCommasCheck =>
            T(
                "Kommas ans Zeilenende (TrailingCommas)\r\n– deaktiviert = führende Kommas",
                "Commas at end of line (TrailingCommas)\r\n– disabled = leading commas");

        public static string SpaceAfterExpandedCommaCheck =>
            T(
                "Leerzeichen nach Komma in Listen (SpaceAfterExpandedComma)",
                "Space after comma in lists (SpaceAfterExpandedComma)");

        public static string UppercaseKeywordsCheck =>
            T("Keywords in Großbuchstaben (UppercaseKeywords)", "Keywords in uppercase (UppercaseKeywords)");

        public static string KeywordStandardizationCheck =>
            T("Keywords standardisieren (KeywordStandardization)", "Standardize keywords (KeywordStandardization)");

        public static string ResetButton =>
            T("Standard wiederherstellen", "Restore defaults");

        public static string CancelButton =>
            T("Abbrechen", "Cancel");
    }
}
