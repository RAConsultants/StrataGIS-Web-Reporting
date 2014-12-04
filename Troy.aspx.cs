using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.GridControls;
using Infragistics.WebUI;
using Newtonsoft.Json;

public partial class Troy: System.Web.UI.Page
{
    SqlConnection dataConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TroyServerConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            fillLists();

            wdpStart.Date = DateTime.Today.AddDays(-8);
            wdpEnd.Date = DateTime.Today.AddDays(-1);
            setGrids("Start");
        }

    }

    protected void setGrids(string input)
    {
        wdgDaily.ClearDataSource();
        wdgDaily.DataBind();
        wdgWeekly.ClearDataSource();
        wdgWeekly.DataBind();
        wdgMonthly.ClearDataSource();
        wdgMonthly.DataBind();
        hdgEnterExit.ClearDataSource();
        hdgEnterExit.DataBind();
        wdgTrip.ClearDataSource();
        wdgTrip.DataBind();
        wdgTotal.ClearDataSource();
        wdgTotal.DataBind();


        wdgDaily.Visible = false;
        wdgWeekly.Visible = false;
        wdgMonthly.Visible = false;
        hdgEnterExit.Visible = false;
        wdgTrip.Visible = false;
        wdgTotal.Visible = false;
        hdgTotal.Visible = false;
        divCharts.Visible = false;
        hdgDailyG.Visible = false;

        switch ( input )
        {
            case "Daily":
                wdgDaily.Visible = true;
                break;
            case "Weekly":
                wdgWeekly.Visible = true;
                break;
            case "Monthly":
                wdgMonthly.Visible = true;
                break;
            case "EnterExit":
                hdgEnterExit.Visible = true;
                break;
            case "Trip":
                wdgTrip.Visible = true;
                break;
            case "Total":
                wdgTotal.Visible = true;
                break;
            case "TotalG":
                hdgTotal.Visible = true;
                divCharts.Visible = true;
                break;
            case "DailyG":
                hdgDailyG.Visible = true;
                break;

        }
    }

    protected void fillLists()
    {
        dataConn.Open();
        string sqlquery = "select VehicleId from dbo.Stratagis_Vehicle_View order by VehicleId";
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        SqlDataReader reader = command.ExecuteReader();
        try
        {
            lbVehicles.Items.Add("All");
            lbVehicles.SelectedIndex = 0;
            while ( reader.Read() )
            {
                lbVehicles.Items.Add(reader[0].ToString());
                //cklVehicles.Items.Add(reader[0].ToString());
            }
        }
        finally
        {
            reader.Close();
        }
        dataConn.Close();

        dataConn.Open();
        sqlquery = "select Department from dbo.Stratagis_Vehicle_View group by Department order by Department";
        command = new SqlCommand(sqlquery, dataConn);
        reader = command.ExecuteReader();
        try
        {
            lbDept.Items.Add("All");
            lbDept.SelectedIndex = 0;
            while ( reader.Read() )
            {
                lbDept.Items.Add(reader[0].ToString());
                //cklVehicles.Items.Add(reader[0].ToString());
            }
        }
        finally
        {
            reader.Close();
        }
        dataConn.Close();
    }


    protected void btnRun_Click(object sender, EventArgs e)
    {
        switch ( ddReportType.SelectedItem.Value )
        {
            case "Daily":
                buildDaily();
                break;
            case "Total":
                buildTotal();
                break;
            case "DailyG":
                buildDailyG();
                break;
            case "TotalG":
                buildTotalG();
                break;
            case "EnterExit":
                buildEnterExit();
                break;
            case "Trip":
                buildTrip();
                break;
        }
    }

    private void buildDaily()
    {
        setGrids("Daily");

        dataConn.Open();
        string sqlquery = "select Replace(Convert(date,Date),' 12:00:00 AM','') as Date ";
        sqlquery += " , Department as Dpt, VehicleId as Vehicle ";
        sqlquery += " , HoursRunning as HrsRun ";
        sqlquery += " , HoursIdle as HrsIdle ";
        sqlquery += " , HoursOff as HrsOff ";
        sqlquery += " , DIHours as DIHrs ";
        sqlquery += " , round(TotalMiles,2) as Miles ";
        sqlquery += " , AvgSpeed, round(MaxSpeed,2) as MaxSpeed ";
        sqlquery += " , OnTime as OnTime ";
        sqlquery += " , OffTime as OffTime ";
        sqlquery += " , MILCodes ";
        string sqlFrom = "  from dbo.Stratagis_Report_Daily ";
        string sqlOrder = " order by DateYear, DateMonth, DateDay, VehicleId";
        string whereClause = " where ";
        whereClause += "Date >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += "and " + vehicleWhere();
        }


        sqlquery += " " + sqlFrom;

        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();


        wdgDaily.DataSource = ds;
        wdgDaily.DataBind();

    }

    private void buildTotal()
    {

        setGrids("Total");


        dataConn.Open();
        string sqlquery = " SELECT Department as Dpt ";
        sqlquery += " ,VehicleId as Vehicle ";
        sqlquery += " ,ROUND(SUM(HoursRunning), 2) AS HrsRun ";
        sqlquery += "  ,ROUND(SUM(HoursIdle), 2) AS HrsIdle ";
        sqlquery += "  ,ROUND(SUM(HoursOff), 2) AS HrsOff ";
        sqlquery += "  ,ROUND(sum(ROUND(ISNULL(DIHours, 0), 2)), 2) AS DIHrs ";
        sqlquery += "  ,ROUND(SUM(TotalMiles), 2) AS Miles ";
        sqlquery += "  ,round(SUM(TotalMiles) / SUM(HoursRunning), 2) AS AvgSpeed ";
        sqlquery += " ,ROUND(max(MaxSpeed), 2) AS MaxSpeed ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OnTime) AS float)) AS datetime)), 100)   AS OnTime ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OffTime) AS float)) AS datetime)), 100)   AS OffTime ";
        sqlquery += " ,STUFF  ((SELECT N', ' + CAST(Replace(Replace(MILCodes, '#' + b.VehicleId + '#', ''), 'v', '') AS VarChar(150))   FROM   dbo.Stratagis_Report_Daily b  WHERE b.MILCodes <> '' AND b.MILCodes IS NOT NULL  AND   a.VehicleId = b.VehicleId AND   ";
        sqlquery += "  b.Date >=  Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   b.Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') FOR XML PATH('')/*,TYPE*/ ), 1, 2, '') as MILCodes ";
        sqlquery += " FROM [dbo].[Stratagis_Report_Daily] a ";
        string whereClause = " where Date>=  Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";
        string sqlOrder = " group by Department,VehicleId ";
        sqlOrder += "  order by Department,VehicleId ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
            sqlquery += whereClause;
        }



        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();

        wdgTotal.DataSource = ds;
        wdgTotal.DataBind();
    }


    private void buildTotalG()
    {
        setGrids("TotalG");
        string sqlquery = "";
        string whereClause = "";
        string sqlOrder = "";
        DataSet dataSet = new DataSet();
        DataTable table;

        #region Chart Tables

        DataTable deptMiles = new DataTable();
        deptMiles.Columns.Add("Dpt", typeof(String));
        deptMiles.Columns.Add("Department", typeof(decimal));

        DataTable VMiles = new DataTable();
        VMiles.Columns.Add("VehicleId", typeof(String));
        VMiles.Columns.Add("Vehicle", typeof(decimal));

        DataTable deptRun = new DataTable();
        deptRun.Columns.Add("Dpt", typeof(String));
        deptRun.Columns.Add("Department", typeof(decimal));

        DataTable VRun = new DataTable();
        VRun.Columns.Add("VehicleId", typeof(String));
        VRun.Columns.Add("Vehicle", typeof(decimal));

        DataTable deptIdle = new DataTable();
        deptIdle.Columns.Add("Dpt", typeof(String));
        deptIdle.Columns.Add("Department", typeof(decimal));

        DataTable VIdle = new DataTable();
        VIdle.Columns.Add("VehicleId", typeof(String));
        VIdle.Columns.Add("Vehicle", typeof(decimal));

        DataTable deptMax = new DataTable();
        deptMax.Columns.Add("Dpt", typeof(String));
        deptMax.Columns.Add("Department", typeof(decimal));

        DataTable VMax = new DataTable();
        VMax.Columns.Add("VehicleId", typeof(String));
        VMax.Columns.Add("Vehicle", typeof(decimal));
        #endregion

        #region Grid Tables
        table = new DataTable("Dpts");
        dataSet.Tables.Add(table);
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("HrsRun", typeof(decimal));
        table.Columns.Add("HrsIdle", typeof(decimal));
        table.Columns.Add("HrsOff", typeof(decimal));
        table.Columns.Add("DIHrs", typeof(decimal));
        table.Columns.Add("Miles", typeof(decimal));
        table.Columns.Add("AvgSpeed", typeof(decimal));
        table.Columns.Add("MaxSpeed", typeof(decimal));
        table.Columns.Add("OnTime", typeof(String));
        table.Columns.Add("OffTime", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        table = new DataTable("Vehicles");
        dataSet.Tables.Add(table);
        table.Columns.Add("Dpt", typeof(String));
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("HrsRun", typeof(decimal));
        table.Columns.Add("HrsIdle", typeof(decimal));
        table.Columns.Add("HrsOff", typeof(decimal));
        table.Columns.Add("DIHrs", typeof(decimal));
        table.Columns.Add("Miles", typeof(decimal));
        table.Columns.Add("AvgSpeed", typeof(decimal));
        table.Columns.Add("MaxSpeed", typeof(decimal));
        table.Columns.Add("OnTime", typeof(String));
        table.Columns.Add("OffTime", typeof(String));
        table.Columns.Add("MILCodes", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };
        dataSet.Relations.Add(dataSet.Tables["Dpts"].Columns["ID"], dataSet.Tables["Vehicles"].Columns["Dpt"]);
        #endregion

        #region Department

        dataConn.Open();
        sqlquery = " SELECT Department as Dpt ";
        sqlquery += " ,ROUND(SUM(HoursRunning), 2) AS HrsRun ";
        sqlquery += "  ,ROUND(SUM(HoursIdle), 2) AS HrsIdle ";
        sqlquery += "  ,ROUND(SUM(HoursOff), 2) AS HrsOff ";
        sqlquery += "  ,ROUND(sum(ROUND(ISNULL(DIHours, 0), 2)), 2) AS DIHrs ";
        sqlquery += "  ,ROUND(SUM(TotalMiles), 2) AS Miles ";
        sqlquery += "  ,round(SUM(TotalMiles) / SUM(HoursRunning), 2) AS AvgSpeed ";
        sqlquery += " ,ROUND(max(MaxSpeed), 2) AS MaxSpeed ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OnTime) AS float)) AS datetime)), 100)   AS OnTime ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OffTime) AS float)) AS datetime)), 100)   AS OffTime ";
        sqlquery += " FROM [dbo].[Stratagis_Report_Daily] a ";
        whereClause = " where Date>=  Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";
        sqlOrder = " group by Department ";
        sqlOrder += "  order by Department ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
        }
        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);
        List<decimal> miles = new List<decimal>();
        dataConn.Close();
        decimal maxmiles = 0;
        foreach ( DataRow row in ds.Tables[0].Rows )
        {
            DataRow ddr = dataSet.Tables["Dpts"].NewRow();
            ddr[0] = row[0].ToString();
            ddr[1] = Convert.ToDecimal(row[1]);
            ddr[2] = Convert.ToDecimal(row[2]);
            ddr[3] = Convert.ToDecimal(row[3]);
            ddr[4] = Convert.ToDecimal(row[4]);
            ddr[5] = Convert.ToDecimal(row[5]);
            ddr[6] = Convert.ToDecimal(row[6]);
            ddr[7] = Convert.ToDecimal(row[7]);
            ddr[8] = row[8].ToString();
            ddr[9] = row[9].ToString();
            dataSet.Tables["Dpts"].Rows.Add(ddr);

            DataRow chartRows = deptMiles.NewRow();
            chartRows[0] = row[0].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[5]));
            deptMiles.Rows.Add(chartRows);
            maxmiles = Math.Max(maxmiles, Math.Round(Convert.ToDecimal(row[5])));

            chartRows = deptRun.NewRow();
            chartRows[0] = row[0].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[1]), 1);
            deptRun.Rows.Add(chartRows);

            chartRows = deptIdle.NewRow();
            chartRows[0] = row[0].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[2]), 1);
            deptIdle.Rows.Add(chartRows);

            chartRows = deptMax.NewRow();
            chartRows[0] = row[0].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[7]));
            deptMax.Rows.Add(chartRows);

        }

        ucDeptMilesB.DataSource = deptMiles;
        ucDeptMilesB.Data.SwapRowsAndColumns = true;
        ucDeptMilesB.DataBind();

        ucDeptRun.DataSource = deptRun;
        ucDeptRun.Data.SwapRowsAndColumns = true;
        ucDeptRun.DataBind();

        ucDeptIdle.DataSource = deptIdle;
        ucDeptIdle.Data.SwapRowsAndColumns = true;
        ucDeptIdle.DataBind();

        ucDeptMax.DataSource = deptMax;
        ucDeptMax.Data.SwapRowsAndColumns = true;
        ucDeptMax.DataBind();
        #endregion

        #region Vehicles
        dataConn.Open();
        sqlquery = " SELECT Department as Dpt ";
        sqlquery += " ,VehicleId as Vehicle ";
        sqlquery += " ,ROUND(SUM(HoursRunning), 2) AS HrsRun ";
        sqlquery += "  ,ROUND(SUM(HoursIdle), 2) AS HrsIdle ";
        sqlquery += "  ,ROUND(SUM(HoursOff), 2) AS HrsOff ";
        sqlquery += "  ,ROUND(sum(ROUND(ISNULL(DIHours, 0), 2)), 2) AS DIHrs ";
        sqlquery += "  ,ROUND(SUM(TotalMiles), 2) AS Miles ";
        sqlquery += "  ,round(SUM(TotalMiles) / SUM(HoursRunning), 2) AS AvgSpeed ";
        sqlquery += " ,ROUND(max(MaxSpeed), 2) AS MaxSpeed ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OnTime) AS float)) AS datetime)), 100)   AS OnTime ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OffTime) AS float)) AS datetime)), 100)   AS OffTime ";
        sqlquery += " ,STUFF  ((SELECT N', ' + CAST(Replace(Replace(MILCodes, '#' + b.VehicleId + '#', ''), 'v', '') AS VarChar(150))   FROM   dbo.Stratagis_Report_Daily b  WHERE b.MILCodes <> '' AND b.MILCodes IS NOT NULL  AND   a.VehicleId = b.VehicleId AND   ";
        sqlquery += "  b.Date >=  Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   b.Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') FOR XML PATH('')/*,TYPE*/ ), 1, 2, '') as MILCodes ";
        sqlquery += " FROM [dbo].[Stratagis_Report_Daily] a ";
        whereClause = " where Date>=  Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";
        sqlOrder = " group by Department,VehicleId ";
        sqlOrder += "  order by Department,VehicleId ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
        }
        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        command = new SqlCommand(sqlquery, dataConn);
        ds = new DataSet();
        sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();
        maxmiles = 0;
        foreach ( DataRow row in ds.Tables[0].Rows )
        {
            DataRow ddr = dataSet.Tables["Vehicles"].NewRow();
            ddr[0] = row[0].ToString();
            ddr[1] = row[1].ToString();
            ddr[2] = Convert.ToDecimal(row[2]);
            ddr[3] = Convert.ToDecimal(row[3]);
            ddr[4] = Convert.ToDecimal(row[4]);
            ddr[5] = Convert.ToDecimal(row[5]);
            ddr[6] = Convert.ToDecimal(row[6]);
            ddr[7] = Convert.ToDecimal(row[7]);
            ddr[8] = Convert.ToDecimal(row[8]);
            ddr[9] = row[9].ToString();
            ddr[10] = row[10].ToString();
            ddr[11] = row[11].ToString();
            dataSet.Tables["Vehicles"].Rows.Add(ddr);

            DataRow chartRows = VMiles.NewRow();
            chartRows[0] = row[1].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[6]));
            VMiles.Rows.Add(chartRows);
            maxmiles = Math.Max(maxmiles, Math.Round(Convert.ToDecimal(row[6])));

            chartRows = VRun.NewRow();
            chartRows[0] = row[1].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[2]), 1);
            VRun.Rows.Add(chartRows);

            chartRows = VIdle.NewRow();
            chartRows[0] = row[1].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[3]), 1);
            VIdle.Rows.Add(chartRows);

            chartRows = VMax.NewRow();
            chartRows[0] = row[1].ToString();
            chartRows[1] = Math.Round(Convert.ToDecimal(row[8]));
            VMax.Rows.Add(chartRows);
        }
        //ucVMiles.DataSource = VMiles;
        ////ucDeptMiles.Data.SwapRowsAndColumns = true;
        //ucVMiles.DataBind();
        //ucVMiles.PieChart.Labels.FormatString = "<ITEM_LABEL>: <DATA_VALUE:#> (<PERCENT_VALUE:#>%)";
        ucVMilesB.DataSource = VMiles;
        ucVMilesB.Data.SwapRowsAndColumns = true;
        ucVMilesB.DataBind();

        ucVRun.DataSource = VRun;
        ucVRun.Data.SwapRowsAndColumns = true;
        ucVRun.DataBind();

        ucVIdle.DataSource = VIdle;
        ucVIdle.Data.SwapRowsAndColumns = true;
        ucVIdle.DataBind();

        ucVMax.DataSource = VMax;
        ucVMax.Data.SwapRowsAndColumns = true;
        ucVMax.DataBind();
        #endregion

        hdgTotal.DataSource = dataSet;
        hdgTotal.DataBind();
    }


    private void buildDailyG()
    {
        string sqlquery = "";
        string whereClause = "";
        string sqlOrder = "";
        string sqlFrom = "";

        setGrids("DailyG");
        DataSet dataSet = new DataSet();
        DataTable table;

        #region Grid Tables
        table = new DataTable("Dates");
        dataSet.Tables.Add(table);
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("HrsRun", typeof(decimal));
        table.Columns.Add("HrsIdle", typeof(decimal));
        table.Columns.Add("HrsOff", typeof(decimal));
        table.Columns.Add("DIHrs", typeof(decimal));
        table.Columns.Add("Miles", typeof(decimal));
        table.Columns.Add("AvgSpeed", typeof(decimal));
        table.Columns.Add("MaxSpeed", typeof(decimal));
        table.Columns.Add("OnTime", typeof(String));
        table.Columns.Add("OffTime", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        table = new DataTable("Dpts");
        dataSet.Tables.Add(table);
        table.Columns.Add("Date", typeof(String));
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("Dpt", typeof(String));
        table.Columns.Add("HrsRun", typeof(decimal));
        table.Columns.Add("HrsIdle", typeof(decimal));
        table.Columns.Add("HrsOff", typeof(decimal));
        table.Columns.Add("DIHrs", typeof(decimal));
        table.Columns.Add("Miles", typeof(decimal));
        table.Columns.Add("AvgSpeed", typeof(decimal));
        table.Columns.Add("MaxSpeed", typeof(decimal));
        table.Columns.Add("OnTime", typeof(String));
        table.Columns.Add("OffTime", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        table = new DataTable("Vehicles");
        dataSet.Tables.Add(table);
        table.Columns.Add("Dpt", typeof(String));
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("VehicleId", typeof(String));
        table.Columns.Add("HrsRun", typeof(decimal));
        table.Columns.Add("HrsIdle", typeof(decimal));
        table.Columns.Add("HrsOff", typeof(decimal));
        table.Columns.Add("DIHrs", typeof(decimal));
        table.Columns.Add("Miles", typeof(decimal));
        table.Columns.Add("AvgSpeed", typeof(decimal));
        table.Columns.Add("MaxSpeed", typeof(decimal));
        table.Columns.Add("OnTime", typeof(String));
        table.Columns.Add("OffTime", typeof(String));
        table.Columns.Add("MILCodes", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };
        dataSet.Relations.Add(dataSet.Tables["Dates"].Columns["ID"], dataSet.Tables["Dpts"].Columns["Date"]);
        dataSet.Relations.Add(dataSet.Tables["Dpts"].Columns["ID"], dataSet.Tables["Vehicles"].Columns["Dpt"]);
        #endregion

        #region Date
        dataConn.Open();
        sqlquery = " SELECT Replace(Convert(date,Date),' 12:00:00 AM','') as Date ";
        sqlquery += " ,ROUND(SUM(HoursRunning), 2) AS HrsRun ";
        sqlquery += "  ,ROUND(SUM(HoursIdle), 2) AS HrsIdle ";
        sqlquery += "  ,ROUND(SUM(HoursOff), 2) AS HrsOff ";
        sqlquery += "  ,ROUND(sum(ROUND(ISNULL(DIHours, 0), 2)), 2) AS DIHrs ";
        sqlquery += "  ,ROUND(SUM(TotalMiles), 2) AS Miles ";
        sqlquery += "  ,round(SUM(TotalMiles) / SUM(HoursRunning), 2) AS AvgSpeed ";
        sqlquery += " ,ROUND(max(MaxSpeed), 2) AS MaxSpeed ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OnTime) AS float)) AS datetime)), 100)   AS OnTime ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OffTime) AS float)) AS datetime)), 100)   AS OffTime ";
        sqlquery += " FROM [dbo].[Stratagis_Report_Daily] a ";
        whereClause = " where Date>=  Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";
        sqlOrder = " group by Date ";
        sqlOrder += "  order by Date ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
        }
        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();

        foreach ( DataRow row in ds.Tables[0].Rows )
        {
            DataRow ddr = dataSet.Tables["Dates"].NewRow();
            ddr[0] = row[0].ToString();
            ddr[1] = Convert.ToDecimal(row[1]);
            ddr[2] = Convert.ToDecimal(row[2]);
            ddr[3] = Convert.ToDecimal(row[3]);
            ddr[4] = Convert.ToDecimal(row[4]);
            ddr[5] = Convert.ToDecimal(row[5]);
            ddr[6] = Convert.ToDecimal(row[6]);
            ddr[7] = Convert.ToDecimal(row[7]);
            ddr[8] = row[8].ToString();
            ddr[9] = row[9].ToString();
            dataSet.Tables["Dates"].Rows.Add(ddr);
        }

        #endregion

        #region dept
        dataConn.Open();
        sqlquery = " SELECT Replace(Convert(date,Date),' 12:00:00 AM','') as Date ";
        sqlquery += " ,Department as Dpt ";
        sqlquery += " ,ROUND(SUM(HoursRunning), 2) AS HrsRun ";
        sqlquery += "  ,ROUND(SUM(HoursIdle), 2) AS HrsIdle ";
        sqlquery += "  ,ROUND(SUM(HoursOff), 2) AS HrsOff ";
        sqlquery += "  ,ROUND(sum(ROUND(ISNULL(DIHours, 0), 2)), 2) AS DIHrs ";
        sqlquery += "  ,ROUND(SUM(TotalMiles), 2) AS Miles ";
        sqlquery += "  ,round(SUM(TotalMiles) / SUM(HoursRunning), 2) AS AvgSpeed ";
        sqlquery += " ,ROUND(max(MaxSpeed), 2) AS MaxSpeed ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OnTime) AS float)) AS datetime)), 100)   AS OnTime ";
        sqlquery += " ,CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OffTime) AS float)) AS datetime)), 100)   AS OffTime ";
        sqlquery += " FROM [dbo].[Stratagis_Report_Daily] a ";
        whereClause = " where Date>=  Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";
        sqlOrder = " group by Date, Department ";
        sqlOrder += "  order by Date, Department ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
        }
        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        command = new SqlCommand(sqlquery, dataConn);
        ds = new DataSet();
        sda = new SqlDataAdapter(command);
        sda.Fill(ds);
        dataConn.Close();

        foreach ( DataRow row in ds.Tables[0].Rows )
        {
            DataRow ddr = dataSet.Tables["Dpts"].NewRow();
            ddr[0] = row[0].ToString();
            ddr[1] = row[0].ToString() + row[1].ToString();
            ddr[2] = row[1].ToString();
            ddr[3] = Convert.ToDecimal(row[2]);
            ddr[4] = Convert.ToDecimal(row[3]);
            ddr[5] = Convert.ToDecimal(row[4]);
            ddr[6] = Convert.ToDecimal(row[5]);
            ddr[7] = Convert.ToDecimal(row[6]);
            ddr[8] = Convert.ToDecimal(row[7]);
            ddr[9] = Convert.ToDecimal(row[8]);
            ddr[10] = row[9].ToString();
            ddr[11] = row[10].ToString();
            dataSet.Tables["Dpts"].Rows.Add(ddr);

        }

        #endregion

        #region vehicle
        dataConn.Open();
        sqlquery = "select Replace(Convert(date,Date),' 12:00:00 AM','') as Date ";
        sqlquery += " , Department as Dpt ";
        sqlquery += " , VehicleId as Vehicle ";
        sqlquery += " , HoursRunning as HrsRun ";
        sqlquery += " , HoursIdle as HrsIdle ";
        sqlquery += " , HoursOff as HrsOff ";
        sqlquery += " , DIHours as DIHrs ";
        sqlquery += " , round(TotalMiles,2) as Miles ";
        sqlquery += " , AvgSpeed ";
        sqlquery += " , round(MaxSpeed,2) as MaxSpeed ";
        sqlquery += " , OnTime as OnTime ";
        sqlquery += " , OffTime as OffTime ";
        sqlquery += " , MILCodes ";
        sqlFrom = "  from dbo.Stratagis_Report_Daily ";
        sqlOrder = " order by Date, VehicleId";
        whereClause = " where ";
        whereClause += "Date >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += "and " + vehicleWhere();
        }


        sqlquery += " " + sqlFrom;

        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        command = new SqlCommand(sqlquery, dataConn);
        ds = new DataSet();
        sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();

        foreach ( DataRow row in ds.Tables[0].Rows )
        {
            DataRow ddr = dataSet.Tables["Vehicles"].NewRow();
            ddr[0] = row[0].ToString() + row[1].ToString();
            ddr[1] = row[0].ToString() + row[1].ToString() + row[2].ToString();
            ddr[2] = row[2].ToString();
            ddr[3] = Convert.ToDecimal(row[3]);
            ddr[4] = Convert.ToDecimal(row[4]);
            ddr[5] = Convert.ToDecimal(row[5]);
            ddr[6] = Convert.ToDecimal(row[6]);
            ddr[7] = Convert.ToDecimal(row[7]);
            ddr[8] = Convert.ToDecimal(row[8]);
            ddr[9] = Convert.ToDecimal(row[9]);
            ddr[10] = row[10].ToString();
            ddr[11] = row[11].ToString();
            ddr[12] = row[12].ToString();
            dataSet.Tables["Vehicles"].Rows.Add(ddr);
        }

        #endregion

        hdgDailyG.DataSource = dataSet;
        hdgDailyG.DataBind();
    }

    private void buildTrip()
    {

        setGrids("Trip");

        dataConn.Open();
        string sqlquery = "select Replace(Convert(date,LocalDate),' 12:00:00 AM','') as Date, Department as Dpt, VehicleId as Vehicle, StartLocalTime, StopLocalTime, TripDuration, IdleTime, TripMiles, StopLength, STX, STY ";
        string sqlFrom = "  from dbo.Stratagis_Report_Trip_Mart_View ";
        string sqlOrder = " order by VehicleId, LocalDate, StartObjectId";
        string whereClause = " where ";
        whereClause += "LocalDate >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and LocalDate <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
            // sqlquery += whereClause;
        }

        //int vlcnt = 0;
        //foreach ( ListItem i in cklVehicles.Items )
        //{
        //    if ( i.Selected == true )
        //    {
        //        if ( vlcnt == 0 )
        //        {
        //            whereClause += " and ";
        //            whereClause += " VehicleId in ('" + i.Text;
        //        }
        //        else
        //        {
        //            whereClause += "','" + i.Text;
        //        }
        //        vlcnt += 1;
        //    }
        //}
        //if ( vlcnt > 0 )
        //    whereClause += "') ";

        sqlquery += " " + sqlFrom;

        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();


        wdgTrip.DataSource = ds;
        wdgTrip.DataBind();
    }

    protected void buildEnterExit()
    {

        setGrids("EnterExit");


        dataConn.Open();
        string sqlquery = "select Date as Date, Department as Dpt, Time, VehicleId as Vehicle, EnterExit";
        string sqlFrom = "  from dbo.SD1_Enter_Exit_View ";
        string sqlOrder = " order by  Department, VehicleId, LocalTimeStamp";
        string whereClause = " where ";
        whereClause += "Date >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
        }

        //int vlcnt = 0;
        //foreach ( ListItem i in cklVehicles.Items )
        //{
        //    if ( i.Selected == true )
        //    {
        //        if ( vlcnt == 0 )
        //        {
        //            whereClause += " and ";
        //            whereClause += " VehicleId in ('" + i.Text;
        //        }
        //        else
        //        {
        //            whereClause += "','" + i.Text;
        //        }
        //        vlcnt += 1;
        //    }
        //}
        //if ( vlcnt > 0 )
        //    whereClause += "') ";

        sqlquery += " " + sqlFrom;

        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();

        hdgEnterExit.DataSource = ds;
        hdgEnterExit.DataBind();


        //DataSet dataSet = new DataSet();

        //DataTable table = new DataTable("Years");
        //dataSet.Tables.Add(table);
        //table.Columns.Add("Year", typeof(int));
        //table.PrimaryKey = new DataColumn[] { table.Columns["Year"] };


        //table = new DataTable("Months");
        //dataSet.Tables.Add(table);
        //table.Columns.Add("ID", typeof(int));
        //table.Columns.Add("Month", typeof(int));
        //table.Columns.Add("Year", typeof(int));
        //table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        //table = new DataTable("Days");
        //dataSet.Tables.Add(table);
        //table.Columns.Add("ID", typeof(int));
        //table.Columns.Add("Day", typeof(int));
        //table.Columns.Add("Month", typeof(int));
        //table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        //table = new DataTable("Vehicles");
        //dataSet.Tables.Add(table);
        //table.Columns.Add("ID", typeof(String));
        //table.Columns.Add("VehicleId", typeof(String));
        //table.Columns.Add("Day", typeof(int));
        //table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        //table = new DataTable("EnterExits");
        //dataSet.Tables.Add(table);
        //table.Columns.Add("ID", typeof(String));
        //table.Columns.Add("Time", typeof(String));
        //table.Columns.Add("EnterExit", typeof(String));
        //table.Columns.Add("VehiclesID", typeof(String));
        //table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };


        //// create a data relation
        //dataSet.Relations.Add(dataSet.Tables["Years"].Columns["Year"], dataSet.Tables["Months"].Columns["Year"]);
        //dataSet.Relations.Add(dataSet.Tables["Months"].Columns["ID"], dataSet.Tables["Days"].Columns["Month"]);
        //dataSet.Relations.Add(dataSet.Tables["Days"].Columns["ID"], dataSet.Tables["Vehicles"].Columns["Day"]);
        //dataSet.Relations.Add(dataSet.Tables["Vehicles"].Columns["ID"], dataSet.Tables["EnterExits"].Columns["VehiclesID"]);

        //List<int> years = new List<int>();
        //List<int> months = new List<int>();
        //List<int> days = new List<int>();
        //List<string> vehicles = new List<string>();
        //int count = 0;

        //foreach ( DataRow row in ds.Tables[0].Rows )
        //{
        //    //DateYear as Year,  DateMonth as Month, DateDay as Day, Time, VehicleId as Vehicle, EnterExit
        //    int year = Convert.ToInt32(row[0]);
        //    int month = Convert.ToInt32(row[1]);
        //    int day = Convert.ToInt32(row[2]);
        //    string time = row[3].ToString();
        //    string vehicle = row[4].ToString();
        //    string enterExit = row[5].ToString();
        //    int yearmonth = Convert.ToInt32(year.ToString("0000") + month.ToString("00"));
        //    int yearmonthday = Convert.ToInt32(year.ToString("0000") + month.ToString("00") + day.ToString("00"));
        //    string yearmonthdayvehicle = year.ToString("0000") + month.ToString("00") + day.ToString("00") + vehicle;

        //    if ( !years.Contains(year) )
        //    {
        //        DataRow dr = dataSet.Tables["Years"].NewRow();
        //        dr[0] = year;
        //        dataSet.Tables["Years"].Rows.Add(dr);
        //        years.Add(year);
        //    }

        //    if ( !months.Contains(yearmonth) )
        //    {
        //        DataRow dr = dataSet.Tables["Months"].NewRow();
        //        dr[0] = yearmonth;
        //        dr[1] = month;
        //        dr[2] = year;
        //        dataSet.Tables["Months"].Rows.Add(dr);
        //        months.Add(yearmonth);
        //    }

        //    if ( !days.Contains(yearmonthday) )
        //    {
        //        DataRow dr = dataSet.Tables["Days"].NewRow();
        //        dr[0] = yearmonthday;
        //        dr[1] = day;
        //        dr[2] = yearmonth;
        //        dataSet.Tables["Days"].Rows.Add(dr);
        //        days.Add(yearmonthday);
        //    }

        //    if ( !vehicles.Contains(yearmonthdayvehicle) )
        //    {
        //        DataRow dr = dataSet.Tables["Vehicles"].NewRow();
        //        dr[0] = yearmonthdayvehicle;
        //        dr[1] = vehicle;
        //        dr[2] = yearmonthday;
        //        dataSet.Tables["Vehicles"].Rows.Add(dr);
        //        vehicles.Add(yearmonthdayvehicle);
        //    }
        //    DataRow ddr = dataSet.Tables["EnterExits"].NewRow();
        //    ddr[0] = count;
        //    ddr[1] = time;
        //    ddr[2] = enterExit;
        //    ddr[3] = yearmonthdayvehicle;
        //    dataSet.Tables["EnterExits"].Rows.Add(ddr);
        //    count += 1;
        //}

        //hdgEnterExit.DataSource = buildGrid();
        //hdgEnterExit.DataBind();
        //lblTest.Text = count.ToString();

    }

    DataSet buildGrid()
    {
        dataConn.Open();
        string sqlquery = "select DateYear as Year,  DateMonth as Month, DateDay as Day, Time, VehicleId as Vehicle, EnterExit";
        string sqlFrom = "  from dbo.SD1_Enter_Exit_View ";
        string sqlOrder = " order by  DateYear, DateMonth, DateDay, VehicleId, LocalTimeStamp";
        string whereClause = " where ";
        whereClause += "Date >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

        if ( selectedVehicles().Count > 0 )
        {
            whereClause += " and " + vehicleWhere();
            // sqlquery += whereClause;
        }

        //int vlcnt = 0;
        //foreach ( ListItem i in cklVehicles.Items )
        //{
        //    if ( i.Selected == true )
        //    {
        //        if ( vlcnt == 0 )
        //        {
        //            whereClause += " and ";
        //            whereClause += " VehicleId in ('" + i.Text;
        //        }
        //        else
        //        {
        //            whereClause += "','" + i.Text;
        //        }
        //        vlcnt += 1;
        //    }
        //}
        //if ( vlcnt > 0 )
        //    whereClause += "') ";

        sqlquery += " " + sqlFrom;

        sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();

        DataSet dataSet = new DataSet();
        DataTable table;

        table = new DataTable("Vehicles");
        dataSet.Tables.Add(table);
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("VehicleId", typeof(String));

        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        table = new DataTable("Years");
        dataSet.Tables.Add(table);
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("Year", typeof(int));
        table.Columns.Add("VehiclesID", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };


        table = new DataTable("Months");
        dataSet.Tables.Add(table);
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("Month", typeof(int));
        table.Columns.Add("Year", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

        table = new DataTable("Days");
        dataSet.Tables.Add(table);
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("Day", typeof(int));
        table.Columns.Add("Month", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };


        table = new DataTable("EnterExits");
        dataSet.Tables.Add(table);
        table.Columns.Add("ID", typeof(String));
        table.Columns.Add("Time", typeof(String));
        table.Columns.Add("EnterExit", typeof(String));
        table.Columns.Add("Day", typeof(String));
        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };


        // create a data relation
        dataSet.Relations.Add(dataSet.Tables["Vehicles"].Columns["ID"], dataSet.Tables["Years"].Columns["VehiclesID"]);
        dataSet.Relations.Add(dataSet.Tables["Years"].Columns["ID"], dataSet.Tables["Months"].Columns["Year"]);
        dataSet.Relations.Add(dataSet.Tables["Months"].Columns["ID"], dataSet.Tables["Days"].Columns["Month"]);
        dataSet.Relations.Add(dataSet.Tables["Days"].Columns["ID"], dataSet.Tables["EnterExits"].Columns["Day"]);

        List<string> years = new List<string>();
        List<string> months = new List<string>();
        List<string> days = new List<string>();
        List<string> vehicles = new List<string>();
        int count = 0;

        foreach ( DataRow row in ds.Tables[0].Rows )
        {
            //DateYear as Year,  DateMonth as Month, DateDay as Day, Time, VehicleId as Vehicle, EnterExit
            int year = Convert.ToInt32(row[0]);
            int month = Convert.ToInt32(row[1]);
            int day = Convert.ToInt32(row[2]);
            string time = row[3].ToString();
            string vehicle = row[4].ToString();
            string enterExit = row[5].ToString();
            string vehicleyear = vehicle + year.ToString("0000");
            string vehicleyearmonth = vehicle + year.ToString("0000") + month.ToString("00");
            string vehicleyearmonthday = vehicle + year.ToString("0000") + month.ToString("00") + day.ToString("00");

            if ( !vehicles.Contains(vehicle) )
            {
                DataRow dr = dataSet.Tables["Vehicles"].NewRow();
                dr[0] = vehicle;
                dr[1] = vehicle;
                dataSet.Tables["Vehicles"].Rows.Add(dr);
                vehicles.Add(vehicle);
            }
            if ( !years.Contains(vehicleyear) )
            {
                DataRow dr = dataSet.Tables["Years"].NewRow();
                dr[0] = vehicleyear;
                dr[1] = year;
                dr[2] = vehicle;
                dataSet.Tables["Years"].Rows.Add(dr);
                years.Add(vehicleyear);
            }

            if ( !months.Contains(vehicleyearmonth) )
            {
                DataRow dr = dataSet.Tables["Months"].NewRow();
                dr[0] = vehicleyearmonth;
                dr[1] = month;
                dr[2] = vehicleyear;
                dataSet.Tables["Months"].Rows.Add(dr);
                months.Add(vehicleyearmonth);
            }

            if ( !days.Contains(vehicleyearmonthday) )
            {
                DataRow dr = dataSet.Tables["Days"].NewRow();
                dr[0] = vehicleyearmonthday;
                dr[1] = day;
                dr[2] = vehicleyearmonth;
                dataSet.Tables["Days"].Rows.Add(dr);
                days.Add(vehicleyearmonthday);
            }

            DataRow ddr = dataSet.Tables["EnterExits"].NewRow();
            ddr[0] = count;
            ddr[1] = time;
            ddr[2] = enterExit;
            ddr[3] = vehicleyearmonthday;
            dataSet.Tables["EnterExits"].Rows.Add(ddr);
            count += 1;
        }
        return dataSet;
    }

    protected void btnSearchDaily_Click(object sender, EventArgs e)
    {

        buildDaily();
    }


    protected void btnSearchEnterExit_Click(object sender, EventArgs e)
    {
        buildEnterExit();
    }

    protected void btnTrip_Click(object sender, EventArgs e)
    {
        buildTrip();

    }



    #region Coordinate Lookup and Reverse Geo-Coding
    public double fttom = 0.30480060960121920243840487680975;
    public double RAD = 180.0 / Math.PI;

    private string spc(double x, double y)
    {
        //  ****           KY NORTH
        //SPCC(50,1)=T(84.D0,15.D0)
        //SPCC(50,2)=500000.D0
        //SPCC(50,3)=0.D0
        //SPCC(50,4)=T(37.D0,58.D0)
        //SPCC(50,5)=T(38.D0,58.D0)
        //SPCC(50,6)=37.5D0
        double[] spccKN = { Tfun(84.0, 15.0), 500000.0, 0.0, Tfun(37.0, 58.0), Tfun(38.0, 58.0), 37.5 };
        //  ****           OH SOUTH
        //SPCC(95,1)=82.5D0
        //SPCC(95,2)=600000.D0
        //SPCC(95,3)=0.D0
        //SPCC(95,4)=T(38.D0,44.D0)
        //SPCC(95,5)=T(40.D0,2.D0)
        //SPCC(95,6)=38.D0
        double[] spccOS = { 82.50, 600000.0, 0.0, Tfun(38.0, 44.0), Tfun(40.0, 2.0), 38.0 };

        double[] spcc = spccOS;

        double NORTH = ( y ) * fttom;
        double EAST = ( x ) * fttom;
        //LATITUDE POSITIVE NORTH, LONGITUDE POSITIVE WEST.  ALL ANGLES ARE IN RADIAN MEASURE.

        //ER is equatorial radius of the ellipsoid (= major semiaxis). The Semi-major axis for GRS-80
        //RF is reciprocal of flattening of the ellipsoid.
        //ESQ is the square of first eccentricity of the ellipsoid.
        //CM IS THE CENTRAL MERIDIAN OF THE PROJECTION ZONE
        //EO IS THE FALSE EASTING VALUE AT THE CM
        //NB is false northing for the southernmost parallel. (Meters)
        //FIS, FIN, FIB are respectively the latitudes of the south standard parallel, the north standard parallel, and the southernmost parallel.
        //E is first eccentricity.
        //SINFO = SIN(FO), where FO is the central parallel.
        //K is mapping radius at the equator.
        //RB is mapping radius at the southernmost latitude.
        //KO is scale factor at the central parallel.
        //NO is northing of intersection of central meridian and parallel.
        //G is a constant for computing chord-to-arc corrections.
        //KP IS POINT SCALE FACTOR


        double ER = 6378137.0;
        double RF = 298.257222101;
        double F = 1.0 / RF;
        double ESQ = ( 2 * F - sq(F) );

        double CM = spcc[0] / RAD;
        double EO = spcc[1];
        double NB = spcc[2];
        double FIS = spcc[3] / RAD; //Latitude of SO. STD. Parallel
        double FIN = spcc[4] / RAD; //Latitude of NO. STD. Parallel
        double FIB = spcc[5] / RAD; //Latitude of Southernmost Parallel

        double E = Math.Sqrt(ESQ);
        double SINFS = Math.Sin(FIS);
        double COSFS = Math.Cos(FIS);
        double SINFN = Math.Sin(FIN);
        double COSFN = Math.Cos(FIN);
        double SINFB = Math.Sin(FIB);

        double QS = Qfun(E, SINFS);
        double QN = Qfun(E, SINFN);
        double QB = Qfun(E, SINFB);
        double W1 = Math.Sqrt(1 - ESQ * sq(SINFS));
        double W2 = Math.Sqrt(1 - ESQ * sq(SINFN));
        double SINFO = Math.Log(W2 * COSFS / ( W1 * COSFN )) / ( QN - QS );
        double K = ER * COSFS * Math.Exp(QS * SINFO) / ( W1 * SINFO );
        double RB = K / Math.Exp(QB * SINFO);
        double QO = Qfun(E, SINFO);
        double RO = K / Math.Exp(QO * SINFO);
        double COSFO = Math.Sqrt(1 - sq(SINFO));
        double KO = Math.Sqrt(1 - ESQ * sq(SINFO)) * ( SINFO / COSFO ) * RO / ER;
        double NO = RB + NB - RO;
        double G = sq(1 - ESQ * sq(SINFO)) / ( 2 * sq(ER * KO) ) * ( 1 - ESQ );

        double NPR = RB - NORTH + NB;
        double EPR = EAST - EO;
        double GAM = Math.Atan(EPR / NPR);
        double LON = CM - ( GAM / SINFO );
        double RPT = Math.Sqrt(sq(NPR) + sq(EPR));
        double Q = Math.Log(K / RPT) / SINFO;
        double TEMP = Math.Exp(2.0 * Q);
        double SINE = ( TEMP - 1.0 ) / ( TEMP + 1.0 );

        for ( int i = 0; i < 3; i++ )
        {
            double F1 = ( Math.Log(( 1.0 + SINE ) / ( 1.0 - SINE )) - E * Math.Log(( 1.0 + E * SINE ) / ( 1.0 - E * SINE )) ) / 2.0 - Q;
            double F2 = 1.0 / ( 1.0 - sq(SINE) ) - ESQ / ( 1.0 - ESQ * sq(SINE) );
            SINE = SINE - F1 / F2;
            //lbltest.Text += SINE.ToString() + " ";
        }
        double LAT = Math.Asin(SINE);
        double FI = LAT; //Latitude
        double LAM = LON; //Longitude
        double SINLAT = Math.Sin(FI);
        double COSLAT = Math.Cos(FI);
        double CONV = ( CM - LAM ) * SINFO; //Convergence
        Q = ( Math.Log(( 1 + SINLAT ) / ( 1 - SINLAT )) - E * Math.Log(( 1 + E * SINLAT ) / ( 1 - E * SINLAT )) ) / 2;
        RPT = K / Math.Exp(SINFO * Q);
        double WP = Math.Sqrt(1 - ESQ * sq(SINLAT));
        double KP = WP * SINFO * RPT / ( ER * COSLAT ); //Point Scale Factor

        double Latatude = FI * RAD;
        double Longitude = ( -1 ) * LAM * RAD;

        //lblout.Text = "(LAT, LON) = (" + Latatude.ToString() + ", " + Longitude.ToString() + ")";
        //var request = new Google.Maps.LatLng(Latatude, Longitude);
        return GetLocation(Latatude, Longitude);
    }

    private double Qfun(double E, double S)
    {
        return ( Math.Log(( 1 + S ) / ( 1 - S )) - E * Math.Log(( 1 + E * S ) / ( 1 - E * S )) ) / 2;
    }

    private double Tfun(double X, double Y)
    {
        return X + Y / 60;
    }

    private double sq(double E)
    {
        return Math.Pow(E, 2);
    }

    private string GetLocation(double lat, double lon)
    {

        HttpWebRequest request = default(HttpWebRequest);
        HttpWebResponse response = null;
        StreamReader reader = default(StreamReader);
        string xml = null;

        HttpWebRequest requestA = default(HttpWebRequest);
        HttpWebResponse responseA = null;
        StreamReader readerA = default(StreamReader);
        string xmlA = null;
        string address = "";
        string street = "";
        string city = "";
        string state = "";
        string postal = "";
        string country = "";
        string prevelement = "";

        try
        {
            //Create the web request
            request = (HttpWebRequest)WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat.ToString() + "," + lon.ToString() + "&sensor=false");
            requestA = (HttpWebRequest)WebRequest.Create("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?distance=400&f=json&location=" + lon.ToString() + "," + lat.ToString());
            //Get response
            response = (HttpWebResponse)request.GetResponse();
            responseA = (HttpWebResponse)requestA.GetResponse();
            //Get the response stream into a reader
            reader = new StreamReader(response.GetResponseStream());
            readerA = new StreamReader(responseA.GetResponseStream());
            xml = reader.ReadToEnd();
            xmlA = readerA.ReadToEnd();
            response.Close();
            responseA.Close();

            JsonTextReader jsonread = new JsonTextReader(new StringReader(xmlA));

            while ( jsonread.Read() )
            {
                if ( jsonread.Value != null )
                {
                    //divout.InnerHtml += "Token: " + jsonread.TokenType + " Value: " + jsonread.Value + "<br/>";

                    switch ( prevelement )
                    {
                        case "Address":
                            street = jsonread.Value.ToString();
                            break;
                        case "City":
                            city = jsonread.Value.ToString();
                            break;
                        case "Region":
                            state = jsonread.Value.ToString();
                            break;
                        case "Postal":
                            postal = jsonread.Value.ToString();
                            break;
                        case "CountryCode":
                            country = jsonread.Value.ToString();
                            break;
                        default:
                            break;
                    }
                    prevelement = jsonread.Value.ToString();
                }
            }

            address = street + ", " + city + ", " + state + " " + postal + " " + country;
            if ( street == "" )
            {

                jsonread = new JsonTextReader(new StringReader(xml));

                while ( jsonread.Read() )
                {
                    if ( jsonread.Value != null )
                    {
                        // divout.InnerHtml += "Token: " + jsonread.TokenType + " Value: " + jsonread.Value + "<br/>";

                        switch ( prevelement )
                        {
                            case "formatted_address":
                                address = jsonread.Value.ToString();
                                break;
                            default:
                                break;
                        }
                        prevelement = jsonread.Value.ToString();
                    }
                }
            }

        }
        catch ( Exception ex )
        {
            string Message = "Error: " + ex.ToString();
        }
        finally
        {
            if ( ( response != null ) )
                response.Close();
        }

        return address;

    }

    protected void wdgTrip_InitializeRow(object sender, RowEventArgs e)
    {
        double stx = Convert.ToDouble(e.Row.Items[10].Value);
        double sty = Convert.ToDouble(e.Row.Items[11].Value);
        e.Row.Items[9].Value = spc(stx, sty);
    }

    #endregion

    #region Department and Vehicle Code

    protected void lbDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ( lbDept.SelectedItem.Text == "All" )
            lbVehicles.SelectedIndex = 0;
        else
        {
            List<string> selectedVehicles = new List<string>();
            dataConn.Open();
            string sqlquery = "select VehicleId from dbo.Stratagis_Vehicle_View where  " + deptWhere() + " order by VehicleId";
            SqlCommand command = new SqlCommand(sqlquery, dataConn);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while ( reader.Read() )
                {
                    selectedVehicles.Add(reader[0].ToString());
                }
            }
            finally
            {
                reader.Close();
            }
            dataConn.Close();

            foreach ( ListItem i in lbVehicles.Items )
            {
                if ( selectedVehicles.Contains(i.Text) )
                    i.Selected = true;
                else
                    i.Selected = false;
            }
        }
    }

    protected List<string> selectedDepts()
    {
        List<string> selectedDeptsList = new List<string>();
        bool all = false;

        foreach ( ListItem i in lbDept.Items )
        {
            if ( i.Selected )
            {
                if ( i.Text == "All" )
                    all = true;
                else if ( !all )
                {
                    selectedDeptsList.Add(i.Text);
                }
            }
        }
        return selectedDeptsList;
    }

    protected List<string> selectedVehicles()
    {
        List<string> selectedVehiclesList = new List<string>();
        bool all = false;

        foreach ( ListItem i in lbVehicles.Items )
        {
            if ( i.Selected )
            {
                if ( i.Text == "All" )
                    all = true;
                else if ( !all )
                {
                    selectedVehiclesList.Add(i.Text);
                }
            }
        }
        return selectedVehiclesList;
    }

    protected string vehicleWhere()
    {
        List<string> selectedVehicleList = selectedVehicles();
        if ( selectedVehicleList.Count > 0 )
        {

            string where = " VehicleId in ( ";
            int count = 0;
            foreach ( string i in selectedVehicleList )
            {
                if ( count == 0 )
                    where += "\'" + i + "\'";
                else
                    where += ",\'" + i + "\'";
            }
            where += ")";
            return where;
        }
        else
            return "";
    }

    protected string deptWhere()
    {
        string where = " Department in (\'" + lbDept.SelectedItem.Text + "\') ";
        return where;
    }
    #endregion

}
