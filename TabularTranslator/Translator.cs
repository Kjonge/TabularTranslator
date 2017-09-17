using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace TabularTranslator
{
    public partial class Translator : Form
    {
        private ModelTranslations _modelTranslations;
        private string _jsonFilename;
        private bool _unsavedChanges = false;
        private class Culturelist
        {
            public string Table { get; set; }
            public string ObjectType { get ; set; }
            public string OriginalName { get; set; }
            public string OriginalDescription { get; set; }
            public string OriginalDisplayFolder { get; set; }
            public string TranslatedCaption {
                get
                {
                    switch (ObjectType)
                    {
                        case "Model": return ((Model)this.TranslatedObject).translatedCaption;
                        case "Table": return ((Table)this.TranslatedObject).translatedCaption;
                        case "Column": return ((Column)this.TranslatedObject).translatedCaption;
                        case "Measure": return ((Measure)this.TranslatedObject).translatedCaption;
                        case "Hierarchy": return ((Hierarchy)this.TranslatedObject).translatedCaption;
                        case "Hierarchylevel": return ((Level)this.TranslatedObject).translatedCaption;
                        case "Perspective": return ((Perspective)this.TranslatedObject).translatedCaption;
                        case "Role": return ((Role)this.TranslatedObject).translatedCaption;
                        case "KPI": return "n/a";
                    }
                    return null;  
                }
                set
                {
                    switch (ObjectType)
                    {
                        case "Model":
                            ((Model)this.TranslatedObject).translatedCaption = value; break;
                        case "Table":
                            ((Table)this.TranslatedObject).translatedCaption = value; break;
                        case "Column":
                            ((Column)this.TranslatedObject).translatedCaption = value; break;
                        case "Measure":
                            ((Measure)this.TranslatedObject).translatedCaption = value; break;
                        case "Hierarchy":
                            ((Hierarchy)this.TranslatedObject).translatedCaption = value; break;
                        case "Hierarchylevel":
                            ((Level)this.TranslatedObject).translatedCaption = value; break;
                        case "Perspective":
                            ((Perspective)this.TranslatedObject).translatedCaption = value; break;
                        case "Role":
                            ((Role)this.TranslatedObject).translatedCaption = value; break;
                    }
                }
            }
            public string TranslatedDescription {
                get
                {
                    switch (ObjectType)
                    {
                        case "Model": return ((Model)this.TranslatedObject).translatedDescription;
                        case "Table": return ((Table)this.TranslatedObject).translatedDescription;
                        case "Column": return ((Column)this.TranslatedObject).translatedDescription;
                        case "Measure": return ((Measure)this.TranslatedObject).translatedDescription;
                        case "KPI": return ((Measure)this.TranslatedObject).kpi.translatedDescription;
                        case "Hierarchy": return ((Hierarchy)this.TranslatedObject).translatedDescription;
                        case "Hierarchylevel": return ((Level)this.TranslatedObject).translatedDescription;
                        case "Perspective": return ((Perspective)this.TranslatedObject).translatedDescription;
                        case "Role": return ((Role)this.TranslatedObject).translatedDescription;
                    }
                    return null;
                }
                set
                {
                    switch (ObjectType)
                    {
                        case "Model":
                            ((Model)this.TranslatedObject).translatedDescription = value; break;
                        case "Table":
                            ((Table)this.TranslatedObject).translatedDescription = value; break;
                        case "Column":
                            ((Column)this.TranslatedObject).translatedDescription = value; break;
                        case "Measure":
                            ((Measure)this.TranslatedObject).translatedDescription = value; break;
                        case "Hierarchy":
                            ((Hierarchy)this.TranslatedObject).translatedDescription = value; break;
                        case "Hierarchylevel":
                            ((Level)this.TranslatedObject).translatedDescription = value; break;
                        case "Perspective":
                            ((Perspective)this.TranslatedObject).translatedDescription = value; break;
                        case "Role":
                            ((Role)this.TranslatedObject).translatedDescription = value; break;
                        case "KPI":
                            ((Measure)this.TranslatedObject).kpi.translatedDescription = value; break;
                    }
                }
            }
            public string TranslatedDisplayFolder {
                get {
                    switch (ObjectType) {
                        case "Column": return ((Column)this.TranslatedObject).translatedDisplayFolder;
                        case "Measure": return ((Measure)this.TranslatedObject).translatedDisplayFolder;
                        case "Hierarchy": return ((Hierarchy)this.TranslatedObject).translatedDisplayFolder;
                    }
                    return null;
                }
                set {
                    switch (ObjectType) {
                        case "Column":
                            ((Column)this.TranslatedObject).translatedDisplayFolder = value; break;
                        case "Measure":
                            ((Measure)this.TranslatedObject).translatedDisplayFolder = value; break;
                        case "Hierarchy":
                            ((Hierarchy)this.TranslatedObject).translatedDisplayFolder = value; break;
                    }
                }
            }

            public object TranslatedObject { get; set; }
        }

        public Translator()
        {
            InitializeComponent();
        }

        private ModelTranslations LoadJsonCulturesFile()
        { 
            OpenFileDialog openJsonFileDialog = new OpenFileDialog();
            // openJsonFileDialog.InitialDirectory = "c:\\"; 
            // We don't set initial directory, so it is the same as last operation...
            openJsonFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openJsonFileDialog.FilterIndex = 1;
            openJsonFileDialog.RestoreDirectory = true;

            if (openJsonFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader myStream = null;
                    if ((myStream = new StreamReader(File.OpenRead(openJsonFileDialog.FileName))) != null)
                    {
                        _jsonFilename = openJsonFileDialog.FileName;
                        using (myStream)
                        {
                            JsonSerializer deserializer = new JsonSerializer();
                            ModelTranslations cultures = (ModelTranslations)deserializer.Deserialize(myStream, typeof(ModelTranslations));
                            return cultures;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read json file from disk. Original error: " + ex.Message);
                }
            }
            return null;
        }

        private void SaveJsonCulturesFile(ModelTranslations translations, string filename)
        {
            try
            {
                using (StreamWriter file = File.CreateText(filename))
                { 
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(file, translations);
                    _unsavedChanges = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not save json file to disk. Original error: " + ex.Message);
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //load cultures
            _modelTranslations = LoadJsonCulturesFile();
            //Set up the UI
            if (_modelTranslations != null && _modelTranslations.cultures != null)
            {
                cbCultures.Enabled = true;
                cbCultures.Items.Clear();
                foreach (var culture in _modelTranslations.cultures)
                {
                    cbCultures.Items.Add(culture.name);
                }
                cbCultures.SelectedIndex = 0;
                btnSave.Enabled = true;
            }
        }

        private void LoadDataGrid(string selectedCultureName)
        {
            foreach (var culture in _modelTranslations.cultures)
            {
               //Load selected culture
               if(culture.name == selectedCultureName)
                {
                    var clist = CreateListfromTranslation(culture.translations);
                    dgvTranslations.DataSource = clist;
                }
            }
            //Disable changes to the reference columns
            dgvTranslations.Columns[0].ReadOnly = true; 
            dgvTranslations.Columns[1].ReadOnly = true;
            dgvTranslations.Columns[2].ReadOnly = true;
            dgvTranslations.Columns[3].ReadOnly = true;
            dgvTranslations.Columns[4].ReadOnly = true;


            //Hide the last column which is the reference to the object
            dgvTranslations.Columns[dgvTranslations.ColumnCount - 1].Visible = false;
            dgvTranslations.AutoResizeColumns();
            dgvTranslations.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
        }

        private List<Culturelist> CreateListfromTranslation(Translations translations)
        {
            
            List<Culturelist> clist = new List<Culturelist>();
            //Add model
            Culturelist listItem = new Culturelist();
            listItem.TranslatedObject = translations.model;
            listItem.ObjectType = "Model";
            listItem.Table = "";
            listItem.OriginalName = translations.model.name;
            listItem.OriginalDescription = _modelTranslations.ReferenceCulture.model.name;
            listItem.TranslatedCaption = translations.model.translatedCaption;
            listItem.TranslatedDescription = translations.model.translatedDescription;
            clist.Add(listItem);
            //add tables
            foreach (Table tableRow in translations.model.tables ?? Enumerable.Empty<Table>())
            {
                listItem = new Culturelist();
                listItem.TranslatedObject = tableRow;
                listItem.ObjectType = "Table";
                listItem.Table = tableRow.name;
                listItem.OriginalName = tableRow.name;
                ReferenceTable referenceTable = _modelTranslations.ReferenceCulture.model.tables.First(refTable => refTable.name == tableRow.name);
                listItem.OriginalDescription = referenceTable.description;
                listItem.TranslatedCaption = tableRow.translatedCaption;
                listItem.TranslatedDescription = tableRow.translatedDescription;
                  clist.Add(listItem);
                //Add columns
                foreach (Column columnRow in tableRow.columns ?? Enumerable.Empty<Column>())
                {
                    listItem = new Culturelist();
                    listItem.TranslatedObject = columnRow;
                    listItem.ObjectType = "Column";
                    listItem.Table = tableRow.name;
                    ReferenceColumn referenceColumn = referenceTable.columns.First(refColumn => refColumn.name == columnRow.name);
                    listItem.OriginalDescription = referenceColumn.description;
                    listItem.OriginalDisplayFolder = referenceColumn.displayFolder;
                    listItem.OriginalName = columnRow.name;
                    listItem.TranslatedCaption = columnRow.translatedCaption;
                    listItem.TranslatedDescription = columnRow.translatedDescription;
                    listItem.TranslatedDisplayFolder = columnRow.translatedDisplayFolder;
                    clist.Add(listItem);
                }
                foreach (Hierarchy hierarchyRow in tableRow.hierarchies ?? Enumerable.Empty<Hierarchy>())
                {
                    listItem = new Culturelist();
                    listItem.TranslatedObject = hierarchyRow;
                    listItem.ObjectType = "Hierarchy";
                    listItem.Table = tableRow.name;
                    ReferenceHierarchy referencehierarchy = referenceTable.hierarchies.First(refHierarchy=> refHierarchy.name == hierarchyRow.name);
                    listItem.OriginalDescription = referencehierarchy.description;
                    listItem.OriginalDisplayFolder = referencehierarchy.displayFolder;
                    listItem.OriginalName = hierarchyRow.name;
                    listItem.TranslatedCaption = hierarchyRow.translatedCaption;
                    listItem.TranslatedDescription = hierarchyRow.translatedDescription;
                    listItem.TranslatedDisplayFolder = hierarchyRow.translatedDisplayFolder;
                    clist.Add(listItem);
                    foreach (Level hierarchyLevelRow in hierarchyRow.levels ?? Enumerable.Empty<Level>())
                    {
                        listItem = new Culturelist();
                        listItem.TranslatedObject = hierarchyLevelRow;
                        listItem.ObjectType = "Hierarchylevel";
                        listItem.Table = tableRow.name;
                        ReferenceLevel referenceHierarchyLevel = referencehierarchy.levels.First(refHierarchyLevel => refHierarchyLevel.name == hierarchyLevelRow.name);
                        listItem.OriginalDescription = referenceHierarchyLevel.description;
                        listItem.OriginalName = hierarchyLevelRow.name;
                        listItem.TranslatedCaption = hierarchyLevelRow.translatedCaption;
                        listItem.TranslatedDescription = hierarchyLevelRow.translatedDescription;
                        clist.Add(listItem);
                    }
                }
                foreach (Measure measureRow in tableRow.measures ?? Enumerable.Empty<Measure>())
                {
                    listItem = new Culturelist();
                    listItem.TranslatedObject = measureRow;
                    listItem.ObjectType = "Measure";
                    listItem.Table = tableRow.name;
                    ReferenceMeasure referenceMeasure = referenceTable.measures.First(refMeasure => refMeasure.name == measureRow.name);
                    listItem.OriginalDescription = referenceMeasure.description;
                    listItem.OriginalDisplayFolder = referenceMeasure.displayFolder;
                    listItem.OriginalName = measureRow.name;
                    listItem.TranslatedCaption = measureRow.translatedCaption;
                    listItem.TranslatedDescription = measureRow.translatedDescription;
                    listItem.TranslatedDisplayFolder = measureRow.translatedDisplayFolder;
                    clist.Add(listItem);
                    if (measureRow.kpi != null)
                    {
                        
                        listItem = new Culturelist();
                        listItem.TranslatedObject = measureRow;
                        listItem.ObjectType = "KPI";
                        listItem.Table = tableRow.name;
                        listItem.OriginalDescription = referenceMeasure.kpi.description;
                        listItem.OriginalDisplayFolder = referenceMeasure.kpi.displayFolder;
                        listItem.OriginalName = measureRow.name;
                        listItem.TranslatedCaption = string.Empty;
                        listItem.TranslatedDescription = measureRow.kpi.translatedDescription;
                        listItem.TranslatedDisplayFolder = measureRow.kpi.translatedDisplayFolder;
                        clist.Add(listItem);
                    }
                }
                
            }
            foreach (Perspective perspectiveRow in translations.model.perspectives ?? Enumerable.Empty<Perspective>())
            {
                listItem = new Culturelist();
                listItem.TranslatedObject = perspectiveRow;
                listItem.ObjectType = "Perspective";
                listItem.Table = string.Empty;
                ReferencePerspective referencePerspective = _modelTranslations.ReferenceCulture.model.perspectives.First(refPerspective => refPerspective.name == perspectiveRow.name);
                listItem.OriginalDescription = referencePerspective.description;
                listItem.OriginalName = perspectiveRow.name;
                listItem.TranslatedCaption = perspectiveRow.translatedCaption;
                listItem.TranslatedDescription = perspectiveRow.translatedDescription;
                clist.Add(listItem);
            }

            return clist;
        }

        private void cbCultures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCultures.SelectedItem != null)
                LoadDataGrid(cbCultures.SelectedItem.ToString());
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // dgvTranslations.Rows.Clear();
            _modelTranslations = null;
            _jsonFilename = string.Empty;
            btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveJsonCulturesFile(_modelTranslations, _jsonFilename);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close app
            Application.Exit();
        }

        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (ConfirmCloseFile()) {
                _modelTranslations = null;
                cbCultures.SelectedItem = null;
                cbCultures.Items.Clear();
                cbCultures.Enabled = false;
                btnSave.Enabled = false;
                dgvTranslations.DataSource = null;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            About about = new About();
            about.ShowDialog();
        }

        private void dgvTranslations_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            _unsavedChanges = true;
        }

        private void dgvTranslations_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
            _unsavedChanges = true;
        }
        private bool ConfirmCloseFile() {
            bool result = !_unsavedChanges;
            if (_unsavedChanges) {
                var confirm = MessageBox.Show("There are unsaved changes. Do you want to save them?", "Unsaved changes", MessageBoxButtons.YesNoCancel);
                switch (confirm) {
                    case DialogResult.Yes:
                        SaveJsonCulturesFile(_modelTranslations, _jsonFilename);
                        result = true;
                        break;
                    case DialogResult.No:
                        result = true;
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            return result;
        }
        protected override void OnClosing(CancelEventArgs e) {
            if (!ConfirmCloseFile()) {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void dgvTranslations_KeyUp(object sender, KeyEventArgs e) {
            if ((e.Shift && e.KeyCode == Keys.Insert) || (e.Control && e.KeyCode == Keys.V)) {
                char[] rowSplitter = { '\r', '\n' };
                char[] columnSplitter = { '\t' };
                //get the text from clipboard
                IDataObject dataInClipboard = Clipboard.GetDataObject();
                string stringInClipboard = ((string)dataInClipboard.GetData(DataFormats.Text)).Replace("\r\n", "\n");
                //split it into lines
                string[] rowsInClipboard = stringInClipboard.Split(rowSplitter);

                //get the row and column of selected cell in grid
                int r = dgvTranslations.SelectedCells[0].RowIndex;
                int c = dgvTranslations.SelectedCells[0].ColumnIndex;

                //Retrieve min and max range
                int MinRow = dgvTranslations.SelectedCells[0].RowIndex;
                int MinCol = dgvTranslations.SelectedCells[0].ColumnIndex;
                int MaxRow = MinRow;
                int MaxCol = MinCol;
                for (int i = 1; i < dgvTranslations.SelectedCells.Count; i++ ) {
                    var selCell = dgvTranslations.SelectedCells[i];
                    if (selCell.RowIndex < MinRow) MinRow = selCell.RowIndex;
                    if (selCell.RowIndex > MaxRow) MaxRow = selCell.RowIndex;
                    if (selCell.ColumnIndex < MinCol) MinCol = selCell.ColumnIndex;
                    if (selCell.ColumnIndex > MaxCol) MaxCol = selCell.ColumnIndex;
                }

                bool singleRowInClipboard = rowsInClipboard.Length == 1;
                int rowsToPaste = singleRowInClipboard ? (MaxRow - MinRow + 1) : rowsInClipboard.Length;
                // loop through the lines, split them into cells and place the values in the corresponding cell.
                for (int iRow = 0; iRow < rowsToPaste; iRow++) {

                    int assignRow = MinRow + iRow;
                     //split row into cell values
                    string[] valuesInRow = rowsInClipboard[singleRowInClipboard ? 0 : iRow].Split(columnSplitter);
                    //cycle through cell values
                    for (int iCol = 0; iCol < valuesInRow.Length; iCol++) {
                        int assignColumn = MinCol + iCol;

                        // Check that the cell is within the selection
                        bool inSelection = false;
                        for (int i = 0; i < dgvTranslations.SelectedCells.Count; i++) {
                            var selCell = dgvTranslations.SelectedCells[i];
                            if ((selCell.RowIndex == assignRow || MinRow == MaxRow) 
                                && (selCell.ColumnIndex == assignColumn || MinCol == MaxCol) ) {
                                inSelection = true;
                                break;
                            }
                        }
                        //assign cell value, only if it within columns of the grid
                        if (inSelection
                            && (dgvTranslations.ColumnCount - 1 >= MinCol + iCol)
                            && (dgvTranslations.RowCount - 1 >= MinRow + iRow) ) {
                            dgvTranslations.Rows[assignRow].Cells[assignColumn].Value = valuesInRow[iCol];
                        }
                    }
                }
            
            }
        }
    }
}
