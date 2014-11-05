using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //hdgEnterExit.DataSource = populateGrid();
        //hdgEnterExit.DataBind();        
    }

    //tables Years,Months, Days, Vehicles, EnterExits
    public DataSet populateGrid()
    {
        DataSet dataSet = new DataSet();

        DataTable table = new DataTable("Years");
        table.Columns.Add("Year", typeof(int));
        // table.PrimaryKey = new DataColumn[] { table.Columns["Year"] };
        //IG assign PrimaryKey
        table.Columns.Add("ID", typeof(System.Int32));
        DataColumn[] col = new DataColumn[1];
        col[0] = table.Columns["ID"];
        table.PrimaryKey = col;
        DataRow row = table.NewRow();
        row.SetField<int>("ID", 4);
        row.SetField<int>("Year", 1);
        table.Rows.Add(row);

        row = table.NewRow();
        row.SetField<int>("ID", 3);
        row.SetField<int>("Year", 2);
        table.Rows.Add(row);
        dataSet.Tables.Add(table);

        table = new DataTable("Months");
        table.Columns.Add("ID", typeof(System.Int32));
        table.Columns.Add("Month", typeof(int));
        table.Columns.Add("Year", typeof(int));
        table.PrimaryKey = new DataColumn[] { table.Columns["Month"] };

        row = table.NewRow();
        row.SetField<int>("ID", 1);
        row.SetField<int>("Month", 11);
        row.SetField<int>("Year", 2);
        table.Rows.Add(row);

        row = table.NewRow();
        row.SetField<int>("ID", 2);
        row.SetField<int>("Month", 12);
        row.SetField<int>("Year", 1);
        table.Rows.Add(row);

        row = table.NewRow();
        row.SetField<int>("ID", 8);
        row.SetField<int>("Month", 1);
        row.SetField<int>("Year", 1);
        table.Rows.Add(row);
        dataSet.Tables.Add(table);

        // -----------
        table = new DataTable("Days");

        table.Columns.Add("ID", typeof(System.Int32));
        table.Columns.Add("Day", typeof(int));
        table.Columns.Add("Month", typeof(int));
        table.Columns.Add("Year", typeof(int));
        table.PrimaryKey = new DataColumn[] { table.Columns["Day"] };
        row = table.NewRow();
        row.SetField<int>("ID", 1);
        row.SetField<int>("Day", 5);
        row.SetField<int>("Month", 1);
        row.SetField<int>("Year", 2);
        table.Rows.Add(row);

        row = table.NewRow();
        row.SetField<int>("ID", 1);
        row.SetField<int>("Day", 6);
        row.SetField<int>("Month", 8);
        row.SetField<int>("Year", 1);
        table.Rows.Add(row);
        dataSet.Tables.Add(table);


        dataSet.Relations.Add("HierarchicalLevels1", dataSet.Tables["Years"].Columns["Year"], dataSet.Tables["Months"].Columns["Year"]);
        dataSet.Relations.Add("HierarchicalLevels2", dataSet.Tables["Months"].Columns["ID"], dataSet.Tables["Days"].Columns["Month"]);

        return dataSet;
    }

    protected void button_Click_BindGrid(object sender, EventArgs e)
    {
        hdgEnterExit.DataSource = populateGrid();
        hdgEnterExit.DataBind();
    }
}