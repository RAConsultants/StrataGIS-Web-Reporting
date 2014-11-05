using System;
using System.Configuration;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.GridControls;
using Infragistics.WebUI;

public partial class _Default: System.Web.UI.Page
{
    SqlConnection dataConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["SD1ServerConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            fillLists();

            wdgDaily.Visible = false;
            wdgWeekly.Visible = false;
            wdgMonthly.Visible = false;
            hdgEnterExit.Visible = false;

            wdpStart.Date = DateTime.Today.AddDays(-8);
            wdpEnd.Date = DateTime.Today.AddDays(-1);
        }

    }
    protected void fillLists()
    {
        dataConn.Open();
        string sqlquery = "select VehicleId from dbo.SD1_VEHICLE_VIEW order by VehicleId";
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        SqlDataReader reader = command.ExecuteReader();
        try
        {
            while ( reader.Read() )
            {
                cklVehicles.Items.Add(reader[0].ToString());
            }
        }
        finally
        {
            reader.Close();
        }
        foreach ( ListItem i in cklVehicles.Items )
        {
            i.Selected = true;
        }

        dataConn.Close();
    }

    protected void btnSearchDaily_Click(object sender, EventArgs e)
    {

        wdgDaily.Visible = true;
        wdgWeekly.Visible = false;
        wdgMonthly.Visible = false;
        hdgEnterExit.Visible = false;

        dataConn.Open();
        string sqlquery = "select Replace(Convert(date,Date),' 12:00:00 AM','') as Date, Department as Dpt, VehicleId as Vehicle, HoursRunning as HrsRun, HoursIdle as HrsIdle, HoursOff as HrsOff, DigitalInputHours as DIHrs, round(TotalMiles,2) as Miles, AvgSpeed, round(MaxSpeed,2) as MaxSpeed, OnTime as OnTime, exittime as ExitTime, entertime as EnterTime, OffTime as OffTime, MILCodes ";
        string sqlFrom = "  from dbo.SD1_MART_DAILY_REPORT ";
        string sqlOrder = " order by DateYear, DateMonth, DateDay, VehicleId";
        string whereClause = " where ";
        whereClause += "LocalDate >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and LocalDate <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

        int vlcnt = 0;
        foreach ( ListItem i in cklVehicles.Items )
        {
            if ( i.Selected == true )
            {
                if ( vlcnt == 0 )
                {
                    whereClause += " and ";
                    whereClause += " VehicleId in ('" + i.Text;
                }
                else
                {
                    whereClause += "','" + i.Text;
                }
                vlcnt += 1;
            }
        }
        if ( vlcnt > 0 )
            whereClause += "') ";

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



    protected void btnSearchWeekly_Click(object sender, EventArgs e)
    {

        wdgDaily.Visible = false;
        wdgWeekly.Visible = true;
        wdgMonthly.Visible = false;
        hdgEnterExit.Visible = false;


        dataConn.Open();
        string sqlquery = "select  DateYear as Year, DateMonth as Month, DateWeek as Week, Department as Dpt, VehicleId as Vehicle, HoursRunning as HrsRun, HoursIdle as HrsIdle, HoursOff as HrsOff, DigitalInputHours as DIHrs, round(TotalMiles,2) as Miles, AvgSpeed, round(MaxSpeed,2) as MaxSpeed, OnTime as OnTime, ExitTime as ExitTime, EnterTime as EnterTime, OffTime as OffTime, MILCodes ";
        string sqlFrom = " from  (SELECT a.DateYear, a.DateMonth, a.DateWeek, a.VehicleId, SUM(HoursRunning) AS HoursRunning, SUM(HoursIdle) AS HoursIdle, SUM(HoursOff) AS HoursOff,  ";
        sqlFrom += " sum(ROUND(ISNULL(DigitalInputHours, 0), 2)) AS DigitalInputHours, SUM(TotalMiles) AS TotalMiles, round(SUM(TotalMiles) / SUM(HoursRunning), 2) AS AvgSpeed,  ";
        sqlFrom += " max(b.MaxSpeed) AS MaxSpeed, Department, CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OnTime) AS float)) AS datetime)), 100)  ";
        sqlFrom += " AS OnTime, CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, ExitTime) AS float)) AS datetime)), 100) AS ExitTime, CONVERT(varchar,  ";
        sqlFrom += " CONVERT(time, cast(avg(cast(CONVERT(datetime, EnterTime) AS float)) AS datetime)), 100) AS EnterTime, CONVERT(varchar, CONVERT(time,  ";
        sqlFrom += " cast(avg(cast(CONVERT(datetime, OffTime) AS float)) AS datetime)), 100) AS OffTime, STUFF ";
        sqlFrom += " ((SELECT N', ' + CAST(Replace(Replace(MILCodes, '#' + b.VehicleId + '#', ''), 'v', '') AS VarChar(150)) ";
        sqlFrom += "  FROM   dbo.SD1_DAILY_MART b ";
        sqlFrom += " WHERE b.MILCodes <> '' AND b.MILCodes IS NOT NULL AND b.DateYear = a.DateYear AND b.DateMonth = a.DateMonth AND b.DateWeek = a.DateWeek AND  ";
        sqlFrom += " a.VehicleId = b.VehicleId FOR XML PATH('')/*,TYPE*/ ), 1, 2, '') AS MILCodes ";
        sqlFrom += " FROM  dbo.SD1_DAILY_MART a, dbo.SD1_MART_DAILY_MAX_SPEED b ";
        sqlFrom += " WHERE a.DateDay = b.DateDay AND a.DateMonth = b.DateMonth AND a.DateYear = b.DateYear AND a.VehicleId = b.VehicleId ";
        sqlFrom += " and  a.Date >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   a.Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";
        sqlFrom += " GROUP BY a.DateYear, a.DateMonth, a.DateWeek, a.VehicleId, Department ) as zzz ";
        string sqlOrder = " order by DateYear, DateMonth, DateWeek, VehicleId";
        string whereClause = " where ";
        //whereClause += "LocalDate >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and LocalDate <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";


        int vlcnt = 0;
        foreach ( ListItem i in cklVehicles.Items )
        {
            if ( i.Selected == true )
            {
                if ( vlcnt == 0 )
                {

                    //whereClause += " and ";
                    whereClause += " VehicleId in ('" + i.Text;
                }
                else
                {
                    whereClause += "','" + i.Text;
                }
                vlcnt += 1;
            }
        }

        sqlquery += " " + sqlFrom;

        if ( vlcnt > 0 )
        {
            whereClause += "') ";
            sqlquery += whereClause;
        }

        //sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();

        wdgWeekly.DataSource = ds;
        wdgWeekly.DataBind();

    }

    protected void btnSearchMonthly_Click(object sender, EventArgs e)
    {

        wdgDaily.Visible = false;
        wdgWeekly.Visible = false;
        wdgMonthly.Visible = true;
        hdgEnterExit.Visible = false;


        dataConn.Open();
        string sqlquery = "select DateYear as Year,  DateMonth as Month, Department as Dpt, VehicleId as Vehicle, HoursRunning as HrsRun, HoursIdle as HrsIdle, HoursOff as HrsOff, DigitalInputHours as DIHrs, round(TotalMiles,2) as Miles, AvgSpeed, round(MaxSpeed,2) as MaxSpeed, OnTime as OnTime, ExitTime as ExitTime, EnterTime as EnterTime, OffTime as OffTime, MILCodes ";
        string sqlFrom = "  from (SELECT a.DateYear, a.DateMonth, a.VehicleId, SUM(HoursRunning) AS HoursRunning, SUM(HoursIdle) AS HoursIdle, SUM(HoursOff) AS HoursOff, Sum(DigitalInputHours)  ";
        sqlFrom += " AS DigitalInputHours, SUM(TotalMiles) AS TotalMiles, round(SUM(TotalMiles) / SUM(HoursRunning), 2) AS AvgSpeed, max(b.MaxSpeed) AS MaxSpeed, Department,  ";
        sqlFrom += " CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OnTime) AS float)) AS datetime)), 100) AS OnTime, CONVERT(varchar, CONVERT(time,  ";
        sqlFrom += " cast(avg(cast(CONVERT(datetime, ExitTime) AS float)) AS datetime)), 100) AS ExitTime, CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime,  ";
        sqlFrom += " EnterTime) AS float)) AS datetime)), 100) AS EnterTime, CONVERT(varchar, CONVERT(time, cast(avg(cast(CONVERT(datetime, OffTime) AS float)) AS datetime)),  ";
        sqlFrom += " 100) AS OffTime, STUFF ";
        sqlFrom += " ((SELECT N', ' + CAST(Replace(Replace(MILCodes, '#' + b.VehicleId + '#', ''), 'v', '') AS VarChar(150)) ";
        sqlFrom += "  FROM   dbo.SD1_DAILY_MART b ";
        sqlFrom += " WHERE b.MILCodes <> '' AND b.MILCodes IS NOT NULL AND b.DateYear = a.DateYear AND b.DateMonth = a.DateMonth AND a.VehicleId = b.VehicleId FOR  ";
        sqlFrom += "   XML PATH('')/*,TYPE*/ ), 1, 2, '') AS MILCodes ";
        sqlFrom += " FROM  dbo.SD1_DAILY_MART a, dbo.SD1_MART_DAILY_MAX_SPEED b ";
        sqlFrom += " WHERE a.DateDay = b.DateDay AND a.DateMonth = b.DateMonth AND a.DateYear = b.DateYear AND a.VehicleId = b.VehicleId ";
        sqlFrom += " and  a.Date >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and   a.Date <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "')  ";
        sqlFrom += " GROUP BY a.DateYear, a.DateMonth, a.VehicleId, Department ) as zzz ";
        string sqlOrder = " order by DateYear, DateMonth, VehicleId";
        string whereClause = " where ";
        //whereClause += "LocalDate >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and LocalDate <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";


        int vlcnt = 0;
        foreach ( ListItem i in cklVehicles.Items )
        {
            if ( i.Selected == true )
            {
                if ( vlcnt == 0 )
                {
                    //whereClause += " and ";
                    whereClause += " VehicleId in ('" + i.Text;
                }
                else
                {
                    whereClause += "','" + i.Text;
                }
                vlcnt += 1;
            }
        }

        sqlquery += " " + sqlFrom;

        if ( vlcnt > 0 )
        {

            whereClause += "') ";
            sqlquery += whereClause;
        }
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();
        wdgMonthly.DataSource = ds;
        wdgMonthly.DataBind();

    }

    protected void btnSearchEnterExit_Click(object sender, EventArgs e)
    {
        wdgDaily.Visible = false;
        wdgWeekly.Visible = false;
        wdgMonthly.Visible = false;
        hdgEnterExit.Visible = true;


        //dataConn.Open();
        //string sqlquery = "select DateYear as Year,  DateMonth as Month, DateDay as Day, Time, VehicleId as Vehicle, EnterExit";
        //string sqlFrom = "  from dbo.SD1_Enter_Exit_View ";
        //string sqlOrder = " order by  DateYear, DateMonth, DateDay, VehicleId, LocalTimeStamp";
        //string whereClause = " where ";
        //whereClause += "LocalDate >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and LocalDate <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

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

        //sqlquery += " " + sqlFrom;

        //sqlquery += whereClause;
        //sqlquery += " " + sqlOrder;
        //SqlCommand command = new SqlCommand(sqlquery, dataConn);
        //DataSet ds = new DataSet();
        //SqlDataAdapter sda = new SqlDataAdapter(command);
        //sda.Fill(ds);

        //dataConn.Close();

        ////hdgEnterExit.DataSource = ds;
        ////hdgEnterExit.DataBind();


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

        hdgEnterExit.DataSource = buildGrid();
        hdgEnterExit.DataBind();
        //lblTest.Text = count.ToString();
    }

    DataSet buildGrid()
    {
        dataConn.Open();
        string sqlquery = "select DateYear as Year,  DateMonth as Month, DateDay as Day, Time, VehicleId as Vehicle, EnterExit";
        string sqlFrom = "  from dbo.SD1_Enter_Exit_View ";
        string sqlOrder = " order by  DateYear, DateMonth, DateDay, VehicleId, LocalTimeStamp";
        string whereClause = " where ";
        whereClause += "LocalDate >= Convert(date,'" + wdpStart.Date.ToString("yyyy-MM-dd") + "') and LocalDate <= Convert(date,'" + wdpEnd.Date.ToString("yyyy-MM-dd") + "') ";

        int vlcnt = 0;
        foreach ( ListItem i in cklVehicles.Items )
        {
            if ( i.Selected == true )
            {
                if ( vlcnt == 0 )
                {
                    whereClause += " and ";
                    whereClause += " VehicleId in ('" + i.Text;
                }
                else
                {
                    whereClause += "','" + i.Text;
                }
                vlcnt += 1;
            }
        }
        if ( vlcnt > 0 )
            whereClause += "') ";

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


}
