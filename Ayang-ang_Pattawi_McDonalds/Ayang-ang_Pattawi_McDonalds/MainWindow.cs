using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Ayang_ang_Pattawi_McDonalds {
    public partial class MainWindow : Form {

        // === Make borderless form movable === //
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        // ===================================== //

        private DataTable dataTableOrders;

        // Keep a list of CheckBoxes objects of the add-ons
        private List<CheckBox> addOnsCheckBoxes;

        // Keep a list of the selectedAddOns to be added to the order array.
        private List<String> selectedAddOns;

        // Data array to be passed in to the DataTable 
        private String[] order;

        public MainWindow() {
            InitializeComponent();

            foreach (Control control in flpCurrentOrder.Controls) {
                control.Padding = new Padding(0, 8, 0, 8);
            }

            order = new string[4];

            addOnsCheckBoxes = new List<CheckBox> {
                cbAddOnFlurOre, cbAddOnMushSoup, cbAddOnPlainRice,
                cbAddOnGravy, cbAddOnExtraLettuce
            };

            selectedAddOns = new List<String>();

            flpOrderOptions.Enabled = false;

            dataTableOrders = new DataTable();
            dataTableOrders.Columns.Add("order");
            dataTableOrders.Columns.Add("fries");
            dataTableOrders.Columns.Add("drink");
            dataTableOrders.Columns.Add("add_on");

            dataGridOrders.DataSource = dataTableOrders;

            dataGridOrders.Columns[0].HeaderText = "Order";
            dataGridOrders.Columns[1].HeaderText = "Fries";
            dataGridOrders.Columns[2].HeaderText = "Drink";
            dataGridOrders.Columns[3].HeaderText = "Add On";

            dataGridOrders.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridOrders.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridOrders.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridOrders.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Make borderless form movable
        private void panelTopBar_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Enables or disables the order buttons in the main menu.
        /// </summary>
        /// <param name="toggleTo"></param>
        private void toggleOrderButtons(bool toggleTo) {
            foreach (Control control in flpOrderMenu.Controls) {
                Panel panel = (Panel)control;
                foreach (Control btnControl in panel.Controls) {
                    if (btnControl is Button) {
                        Button button = (Button)btnControl;
                        button.Enabled = toggleTo;
                    }
                }
            }
        }

        private void btnOrderClick(object sender, EventArgs e) {
            btnSave.Enabled = true;

            toggleOrderButtons(false);

            Button clickedButton = (Button)sender;

            flpOrderOptions.Enabled = true;

            lblCurrentOrder.Text = $"{clickedButton.Tag}";

            order[0] = clickedButton.Tag.ToString();
        }

        private void friesClick(object sender, EventArgs e) {
            RadioButton checkRadioButton = (RadioButton)sender;

            lblCurFries.Text = $"{checkRadioButton.Tag.ToString()} fries";

            order[1] = checkRadioButton.Tag.ToString();
        }

        private void drinkClick(object sender, EventArgs e) {
            RadioButton clickedRadioButton = (RadioButton)sender;

            lblCurDrink.Text = clickedRadioButton.Tag.ToString();

            order[2] = clickedRadioButton.Tag.ToString();
        }

        private void AddOnClicked(object sender, EventArgs e) {
            CheckBox clickedCheckBox = (CheckBox)sender;

            flpCurOrderAddOn.Controls.Clear();
            foreach (CheckBox cb in addOnsCheckBoxes) {
                if (cb == clickedCheckBox) {
                    if (clickedCheckBox.Checked && !selectedAddOns.Contains(clickedCheckBox.Text)) {
                        selectedAddOns.Add(clickedCheckBox.Tag.ToString());
                    }

                    if (!clickedCheckBox.Checked && selectedAddOns.Contains(clickedCheckBox.Text)) {
                        selectedAddOns.Remove(clickedCheckBox.Tag.ToString());
                    }
                }

                if (cb.Checked) {

                    Label label = new Label() {
                        AutoSize = true,
                        Dock = DockStyle.Left,
                        Font = new Font("Consolas", 10, FontStyle.Regular),
                        Text = cb.Tag.ToString()
                    };

                    flpCurOrderAddOn.Controls.Add(label);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPageOrders;

            String addOns = "";
            foreach (String selectedAddOn in selectedAddOns) {
                addOns += selectedAddOn + ", ";
            }

            if (!String.IsNullOrWhiteSpace(addOns)) {
                addOns = addOns.Remove(addOns.Length - 2);
            }

            order[3] = addOns;

            for (int i = 0; i < nupOrderAmount.Value; i++) {
                dataTableOrders.Rows.Add(order);
            }

            reset();
        }

        private void reset() {
            btnSave.Enabled = false;

            toggleOrderButtons(true);

            // Reset the order String array.
            order = new string[4];

            // Clear the selectedAddOns String LList.
            selectedAddOns.Clear();

            foreach (Control friesControl in friesPanel.Controls) {
                if (friesControl is RadioButton) {
                    RadioButton rbFries = (RadioButton)friesControl;
                    rbFries.Checked = false;
                }
            }

            foreach (Control drinkControl in drinksPanel.Controls) {
                if (drinkControl is RadioButton) {
                    RadioButton rbDrink = (RadioButton)drinkControl;
                    rbDrink.Checked = false;
                }
            }

            foreach (CheckBox addon in addOnsCheckBoxes) {
                addon.Checked = false;
            }

            nupOrderAmount.Value = 1;

            lblCurrentOrder.Text = "";
            lblCurFries.Text = "";
            lblCurDrink.Text = "";

            flpCurOrderAddOn.Controls.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void cbAddOnFlurOre_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
