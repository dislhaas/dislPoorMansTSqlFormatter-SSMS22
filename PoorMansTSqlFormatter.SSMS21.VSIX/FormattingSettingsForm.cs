using PoorMansTSqlFormatterLib.Formatters;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PoorMansTSqlFormatter.SSMS21.VSIX
{
    /// <summary>
    /// Einfacher WinForms-Dialog für die Formatierungsoptionen der Bibliothek.
    /// Ergebnis steht nach OK in <see cref="Result"/>, die Werte werden in
    /// <see cref="FormattingSettings.Save"/> persistiert.
    /// </summary>
    internal sealed class FormattingSettingsForm : Form
    {
        private readonly ComboBox cmbIndent;
        private readonly NumericUpDown numSpacesPerTab;
        private readonly NumericUpDown numMaxLineWidth;
        private readonly NumericUpDown numStatementBreaks;
        private readonly NumericUpDown numClauseBreaks;
        private readonly CheckBox chkExpandCommaLists;
        private readonly CheckBox chkTrailingCommas;
        private readonly CheckBox chkSpaceAfterExpandedComma;
        private readonly CheckBox chkExpandBooleanExpressions;
        private readonly CheckBox chkExpandBetweenConditions;
        private readonly CheckBox chkExpandCaseStatements;
        private readonly CheckBox chkUppercaseKeywords;
        private readonly CheckBox chkBreakJoinOnSections;
        private readonly CheckBox chkKeywordStandardization;
        private readonly CheckBox chkExpandInLists;

        public FormattingSettingsForm(TSqlStandardFormatterOptions options)
        {
            Text = Localizer.SettingsTitle;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9F);
            ClientSize = new Size(490, 610);

            cmbIndent = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbIndent.Items.AddRange(Localizer.IndentChoices);

            numSpacesPerTab = NewNumeric(1, 8, 4);
            numMaxLineWidth = NewNumeric(60, 999, 999);
            numStatementBreaks = NewNumeric(0, 3, 2);
            numClauseBreaks = NewNumeric(0, 3, 1);

            chkExpandCommaLists = NewCheck(Localizer.ExpandCommaListsCheck);
            chkExpandBooleanExpressions = NewCheck(Localizer.ExpandBooleanExpressionsCheck);
            chkExpandCaseStatements = NewCheck(Localizer.ExpandCaseStatementsCheck);
            chkExpandInLists = NewCheck(Localizer.ExpandInListsCheck);
            chkExpandBetweenConditions = NewCheck(Localizer.ExpandBetweenConditionsCheck);
            chkBreakJoinOnSections = NewCheck(Localizer.BreakJoinOnSectionsCheck);
            chkTrailingCommas = NewCheck(Localizer.TrailingCommasCheck);
            chkSpaceAfterExpandedComma = NewCheck(Localizer.SpaceAfterExpandedCommaCheck);
            chkUppercaseKeywords = NewCheck(Localizer.UppercaseKeywordsCheck);
            chkKeywordStandardization = NewCheck(Localizer.KeywordStandardizationCheck);

            BuildLayout();
            LoadFromOptions(options);

            var btnReset = new Button { Text = Localizer.ResetButton, Width = 180, Location = new Point(12, 578) };
            var btnOk = new Button { Text = "OK", Width = 90, DialogResult = DialogResult.OK, Location = new Point(244, 578) };
            var btnCancel = new Button { Text = Localizer.CancelButton, Width = 90, DialogResult = DialogResult.Cancel, Location = new Point(342, 578) };

            btnReset.Click += (s, e) => LoadFromOptions(new TSqlStandardFormatterOptions());
            btnOk.Click += (s, e) => { Result = ApplyToOptions(); };

            Controls.Add(btnReset);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        /// <summary>Vom Nutzer bestätigte Optionen (nur nach OK gesetzt).</summary>
        public TSqlStandardFormatterOptions Result { get; private set; }

        private void BuildLayout()
        {
            // --- Einrückung ---
            var gbIndent = new GroupBox { Text = Localizer.IndentGroup, Location = new Point(12, 12), Size = new Size(466, 110) };
            AddLabel(gbIndent, Localizer.IndentLabel, 14, 28);
            gbIndent.Controls.Add(cmbIndent);
            cmbIndent.Location = new Point(210, 24);
            cmbIndent.Width = 150;
            AddLabel(gbIndent, Localizer.SpacesPerTabLabel, 14, 58);
            gbIndent.Controls.Add(numSpacesPerTab);
            numSpacesPerTab.Location = new Point(210, 54);
            AddLabel(gbIndent, Localizer.MaxLineWidthLabel, 14, 88);
            gbIndent.Controls.Add(numMaxLineWidth);
            numMaxLineWidth.Location = new Point(210, 84);

            // --- Leerzeilen ---
            var gbBreaks = new GroupBox { Text = Localizer.BlankLinesGroup, Location = new Point(12, 130), Size = new Size(466, 90) };
            AddLabel(gbBreaks, Localizer.StatementBreaksLabel, 14, 26);
            gbBreaks.Controls.Add(numStatementBreaks);
            numStatementBreaks.Location = new Point(300, 22);
            AddLabel(gbBreaks, Localizer.ClauseBreaksLabel, 14, 56);
            gbBreaks.Controls.Add(numClauseBreaks);
            numClauseBreaks.Location = new Point(300, 52);

            // --- Listen / Aufklappen ---
            var gbExpand = new GroupBox { Text = Localizer.ExpandListsGroup, Location = new Point(12, 228), Size = new Size(466, 150) };
            PositionCheck(gbExpand, chkExpandCommaLists, 14, 22);
            PositionCheck(gbExpand, chkExpandBooleanExpressions, 14, 60);
            PositionCheck(gbExpand, chkExpandCaseStatements, 14, 98);
            PositionCheck(gbExpand, chkExpandInLists, 230, 22);
            PositionCheck(gbExpand, chkExpandBetweenConditions, 230, 60);
            PositionCheck(gbExpand, chkBreakJoinOnSections, 230, 98);

            // --- Kommas ---
            var gbComma = new GroupBox { Text = Localizer.CommaGroup, Location = new Point(12, 386), Size = new Size(466, 92) };
            PositionCheck(gbComma, chkTrailingCommas, 14, 22);
            PositionCheck(gbComma, chkSpaceAfterExpandedComma, 14, 64);

            // --- Keywords ---
            var gbKeywords = new GroupBox { Text = Localizer.KeywordGroup, Location = new Point(12, 486), Size = new Size(466, 80) };
            PositionCheck(gbKeywords, chkUppercaseKeywords, 14, 24);
            PositionCheck(gbKeywords, chkKeywordStandardization, 14, 52);

            Controls.Add(gbIndent);
            Controls.Add(gbBreaks);
            Controls.Add(gbExpand);
            Controls.Add(gbComma);
            Controls.Add(gbKeywords);
        }

        private void LoadFromOptions(TSqlStandardFormatterOptions o)
        {
            if (o.IndentString == "  ") cmbIndent.SelectedIndex = 1;
            else if (o.IndentString == "    ") cmbIndent.SelectedIndex = 2;
            else cmbIndent.SelectedIndex = 0;

            numSpacesPerTab.Value = o.SpacesPerTab;
            numMaxLineWidth.Value = o.MaxLineWidth;
            numStatementBreaks.Value = o.NewStatementLineBreaks;
            numClauseBreaks.Value = o.NewClauseLineBreaks;
            chkExpandCommaLists.Checked = o.ExpandCommaLists;
            chkExpandBooleanExpressions.Checked = o.ExpandBooleanExpressions;
            chkExpandCaseStatements.Checked = o.ExpandCaseStatements;
            chkExpandInLists.Checked = o.ExpandInLists;
            chkExpandBetweenConditions.Checked = o.ExpandBetweenConditions;
            chkBreakJoinOnSections.Checked = o.BreakJoinOnSections;
            chkTrailingCommas.Checked = o.TrailingCommas;
            chkSpaceAfterExpandedComma.Checked = o.SpaceAfterExpandedComma;
            chkUppercaseKeywords.Checked = o.UppercaseKeywords;
            chkKeywordStandardization.Checked = o.KeywordStandardization;
        }

        private TSqlStandardFormatterOptions ApplyToOptions()
        {
            string indent;
            if (cmbIndent.SelectedIndex == 1) indent = "  ";
            else if (cmbIndent.SelectedIndex == 2) indent = "    ";
            else indent = "\t";

            return new TSqlStandardFormatterOptions
            {
                IndentString = indent,
                SpacesPerTab = (int)numSpacesPerTab.Value,
                MaxLineWidth = (int)numMaxLineWidth.Value,
                NewStatementLineBreaks = (int)numStatementBreaks.Value,
                NewClauseLineBreaks = (int)numClauseBreaks.Value,
                ExpandCommaLists = chkExpandCommaLists.Checked,
                ExpandBooleanExpressions = chkExpandBooleanExpressions.Checked,
                ExpandCaseStatements = chkExpandCaseStatements.Checked,
                ExpandInLists = chkExpandInLists.Checked,
                ExpandBetweenConditions = chkExpandBetweenConditions.Checked,
                BreakJoinOnSections = chkBreakJoinOnSections.Checked,
                TrailingCommas = chkTrailingCommas.Checked,
                SpaceAfterExpandedComma = chkSpaceAfterExpandedComma.Checked,
                UppercaseKeywords = chkUppercaseKeywords.Checked,
                KeywordStandardization = chkKeywordStandardization.Checked
            };
        }

        private static NumericUpDown NewNumeric(int min, int max, int value)
        {
            return new NumericUpDown { Minimum = min, Maximum = max, Value = value, Width = 60 };
        }

        private static CheckBox NewCheck(string text)
        {
            return new CheckBox { Text = text, AutoSize = true };
        }

        private static void AddLabel(Control parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label { Text = text, AutoSize = true, Location = new Point(x, y) });
        }

        private static void PositionCheck(Control parent, CheckBox cb, int x, int y)
        {
            cb.Location = new Point(x, y);
            cb.Width = 195;
            parent.Controls.Add(cb);
        }
    }
}
