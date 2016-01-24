using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TabularTranslator
{
    public partial class Translator : Form
    {
        private ModelTranslations _modelTranslations;
        private string _jsonFilename;
        private class Culturelist
        {
            public string Table { get; set; }
            public string ObjectType { get ; set; }
            public string OrigionalName { get; set; }
            public string OrigionalDescription { get; set; }
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
        
            public object TranslatedObject { get; set; }
        }

        public Translator()
        {
            InitializeComponent();
        }

        private ModelTranslations LoadJsonCulturesFile()
        { 
            OpenFileDialog openJsonFileDialog = new OpenFileDialog();
            openJsonFileDialog.InitialDirectory = "c:\\";
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
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(file, translations);
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


            //Hide the last column which is the reference to the object
            dgvTranslations.Columns[dgvTranslations.ColumnCount - 1].Visible = false;
            dgvTranslations.AutoResizeColumns();
        }

        private List<Culturelist> CreateListfromTranslation(Translations translations)
        {
            
            List<Culturelist> clist = new List<Culturelist>();
            //Add model
            Culturelist listItem = new Culturelist();
            listItem.TranslatedObject = translations.model;
            listItem.ObjectType = "Model";
            listItem.Table = "";
            listItem.OrigionalName = translations.model.name;
            listItem.OrigionalDescription = _modelTranslations.ReferenceCulture.model.name;
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
                listItem.OrigionalName = tableRow.name;
                ReferenceTable referenceTable = _modelTranslations.ReferenceCulture.model.tables.First(refTable => refTable.name == tableRow.name);
                listItem.OrigionalDescription = referenceTable.description;
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
                    listItem.OrigionalDescription = referenceColumn.description;
                    listItem.OrigionalName = columnRow.name;
                    listItem.TranslatedCaption = columnRow.translatedCaption;
                    listItem.TranslatedDescription = columnRow.translatedDescription;
                    clist.Add(listItem);
                }
                foreach (Hierarchy hierarchyRow in tableRow.hierarchies ?? Enumerable.Empty<Hierarchy>())
                {
                    listItem = new Culturelist();
                    listItem.TranslatedObject = hierarchyRow;
                    listItem.ObjectType = "Hierarchy";
                    listItem.Table = tableRow.name;
                    ReferenceHierarchy referencehierarchy = referenceTable.hierarchies.First(refHierarchy=> refHierarchy.name == hierarchyRow.name);
                    listItem.OrigionalDescription = referencehierarchy.description;
                    listItem.OrigionalName = hierarchyRow.name;
                    listItem.TranslatedCaption = hierarchyRow.translatedCaption;
                    listItem.TranslatedDescription = hierarchyRow.translatedDescription;
                    clist.Add(listItem);
                    foreach (Level hierarchyLevelRow in hierarchyRow.levels ?? Enumerable.Empty<Level>())
                    {
                        listItem = new Culturelist();
                        listItem.TranslatedObject = hierarchyLevelRow;
                        listItem.ObjectType = "Hierarchylevel";
                        listItem.Table = tableRow.name;
                        ReferenceLevel referenceHierarchyLevel = referencehierarchy.levels.First(refHierarchyLevel => refHierarchyLevel.name == hierarchyLevelRow.name);
                        listItem.OrigionalDescription = referenceHierarchyLevel.description;
                        listItem.OrigionalName = hierarchyLevelRow.name;
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
                    listItem.OrigionalDescription = referenceMeasure.description;
                    listItem.OrigionalName = measureRow.name;
                    listItem.TranslatedCaption = measureRow.translatedCaption;
                    listItem.TranslatedDescription = measureRow.translatedDescription;
                    clist.Add(listItem);
                    if (measureRow.kpi != null)
                    {
                        
                        listItem = new Culturelist();
                        listItem.TranslatedObject = measureRow;
                        listItem.ObjectType = "KPI";
                        listItem.Table = tableRow.name;
                        listItem.OrigionalDescription = referenceMeasure.kpi.description;
                        listItem.OrigionalName = measureRow.name;
                        listItem.TranslatedCaption = string.Empty;
                        listItem.TranslatedDescription = measureRow.kpi.translatedDescription;
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
                listItem.OrigionalDescription = referencePerspective.description;
                listItem.OrigionalName = perspectiveRow.name;
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
            _modelTranslations = null;
            cbCultures.SelectedItem = null;
            cbCultures.Items.Clear();
            cbCultures.Enabled = false;
            btnSave.Enabled = false;
            dgvTranslations.DataSource = null;
        }
    }
}
