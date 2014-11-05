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

public partial class GEP : System.Web.UI.Page
{
    SqlConnection dataConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["GEPServerConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
            fillLists();

    }
    protected void fillLists()
    {
        dataConn.Open();
        string sqlquery = "select VehicleId from dbo.StrataGIS_Vehicle_View group by VehicleId order by VehicleId";
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        SqlDataReader reader = command.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                cklVehicles.Items.Add(reader[0].ToString());
            }
        }
        finally
        {
            reader.Close();
        }
        foreach (ListItem i in cklVehicles.Items)
        {
            i.Selected = true;
        }

        sqlquery = "select DateYear from dbo.StrataGIS_Vehicle_View group by DateYear order by DateYear ";
        command = new SqlCommand(sqlquery, dataConn);
        reader = command.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                cklYear.Items.Add(reader[0].ToString());
            }
        }
        finally
        {
            reader.Close();
        }
        foreach (ListItem i in cklYear.Items)
        {
            i.Selected = true;
        }

        sqlquery = "select DateMonth from dbo.StrataGIS_Vehicle_View group by DateMonth order by DateMonth";
        command = new SqlCommand(sqlquery, dataConn);
        reader = command.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                cklMonth.Items.Add(reader[0].ToString());
            }
        }
        finally
        {
            reader.Close();
        }
        foreach (ListItem i in cklMonth.Items)
        {
            i.Selected = true;
        }

        dataConn.Close();
    }

    protected void btnSearchDaily_Click(object sender, EventArgs e)
    {
        gvDaily.Visible = true;
        gvWeekly.Visible = false;
        gvMonthly.Visible = false;
        dataConn.Open();
      //  [Date]
      //,[DateYear]
      //,[DateMonth]
      //,[DateWeek]
      //,[DateDay]
      //,[VehicleId]
      //,[TotalMiles]
      //,[HoursRunning]
      //,[HoursIdle]
      //,[HoursOff]
      //,[AvgSpeed]
      //,[MaxSpeed]
        string sqlquery = "select Replace(Convert(date,Date),' 12:00:00 AM','') as Date, VehicleId as Vehicle, HoursRunning as HrsRun, HoursIdle as HrsIdle, HoursOff as HrsOff,  round(TotalMiles,2) as Miles, AvgSpeed as AvgSpeed, round(MaxSpeed,2) as MaxSpeed";
        string sqlFrom = "  from dbo.StrataGIS_Daily_Report_Mart ";
        string sqlOrder = " order by DateYear, DateMonth, DateDay, VehicleId";
        string whereClause = " where ";
        int yrcnt = 0;
        foreach (ListItem i in cklYear.Items)
        {
            if (i.Selected == true)
            {
                if (yrcnt == 0)
                {
                    whereClause += "DateYear in (" + i.Text ;
                }
                else
                {
                    whereClause += "," + i.Text; 
                }
                yrcnt += 1;
            }
        }
        if (yrcnt > 0)
            whereClause += ") ";

        int mtcnt = 0;
        foreach (ListItem i in cklMonth.Items)
        {
            if (i.Selected == true)
            {
                if (mtcnt == 0)
                {
                    if (yrcnt > 0)
                        whereClause += " and ";
                    whereClause += " DateMonth in (" + i.Text;
                }
                else
                {
                    whereClause += "," + i.Text;
                }
                mtcnt += 1;
            }
        }
        if (mtcnt > 0)
            whereClause += ") ";

        int vlcnt = 0;
        foreach (ListItem i in cklVehicles.Items)
        {
            if (i.Selected == true)
            {
                if (vlcnt == 0)
                {
                    if (yrcnt > 0 || mtcnt>0)
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
        if (vlcnt > 0)
            whereClause += "') ";
        
        sqlquery += " " + sqlFrom;

        if (yrcnt > 0 || mtcnt > 0 || vlcnt > 0)
            sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);       
        
        dataConn.Close();
        gvDaily.DataSource = ds;
        gvDaily.DataBind();

    }

    protected void gvDaily_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Right;

            //e.Row.Cells[0].Width = 80;
            //e.Row.Cells[1].Width = 30;
            //e.Row.Cells[2].Width = 50;
            //e.Row.Cells[3].Width = 50;
            //e.Row.Cells[4].Width = 50;
            //e.Row.Cells[5].Width = 50;
            //e.Row.Cells[6].Width = 50;
            //e.Row.Cells[7].Width = 60;
            //e.Row.Cells[8].Width = 60;
            //e.Row.Cells[9].Width = 60;
            //e.Row.Cells[10].Width = 70;
            //e.Row.Cells[11].Width = 70;
            //e.Row.Cells[12].Width = 70;
            //e.Row.Cells[13].Width = 70;
            //e.Row.Cells[14].Width = 140;

            e.Row.Cells[2].Text = Double.Parse(e.Row.Cells[2].Text.ToString()).ToString("0.00");
            e.Row.Cells[3].Text = Double.Parse(e.Row.Cells[3].Text.ToString()).ToString("0.00");
            e.Row.Cells[4].Text = Double.Parse(e.Row.Cells[4].Text.ToString()).ToString("0.00");
            e.Row.Cells[5].Text = Double.Parse(e.Row.Cells[5].Text.ToString()).ToString("0.00");
            e.Row.Cells[6].Text = Double.Parse(e.Row.Cells[6].Text.ToString()).ToString("0.00");
            e.Row.Cells[7].Text = Double.Parse(e.Row.Cells[7].Text.ToString()).ToString("0.00");
            //e.Row.Cells[8].Text = Double.Parse(e.Row.Cells[8].Text.ToString()).ToString("0.00");
            //e.Row.Cells[9].Text = Double.Parse(e.Row.Cells[9].Text.ToString()).ToString("0.00");
        }
    }



    protected void btnSearchWeekly_Click(object sender, EventArgs e)
    {
        gvDaily.Visible = false;
        gvWeekly.Visible = true;
        gvMonthly.Visible = false;

        dataConn.Open();
        string sqlquery = "select DateYear as Year,DateMonth as Month, DateWeek as Week, VehicleId as Vehicle, HoursRunning as HrsRun, HoursIdle as HrsIdle, HoursOff as HrsOff, round(TotalMiles,2) as Miles, AvgSpeed, round(MaxSpeed,2) as MaxSpeed ";
        string sqlFrom = "  from dbo.StrataGIS_Mart_Weekly_Report ";
        string sqlOrder = " order by DateYear, DateMonth, DateWeek, VehicleId";
        string whereClause = " where ";
        int yrcnt = 0;
        foreach (ListItem i in cklYear.Items)
        {
            if (i.Selected == true)
            {
                if (yrcnt == 0)
                {
                    whereClause += "DateYear in (" + i.Text;
                }
                else
                {
                    whereClause += "," + i.Text;
                }
                yrcnt += 1;
            }
        }
        if (yrcnt > 0)
            whereClause += ") ";

        int mtcnt = 0;
        foreach (ListItem i in cklMonth.Items)
        {
            if (i.Selected == true)
            {
                if (mtcnt == 0)
                {
                    if (yrcnt > 0)
                        whereClause += " and ";
                    whereClause += " DateMonth in (" + i.Text;
                }
                else
                {
                    whereClause += "," + i.Text;
                }
                mtcnt += 1;
            }
        }
        if (mtcnt > 0)
            whereClause += ") ";

        int vlcnt = 0;
        foreach (ListItem i in cklVehicles.Items)
        {
            if (i.Selected == true)
            {
                if (vlcnt == 0)
                {
                    if (yrcnt > 0 || mtcnt > 0)
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
        if (vlcnt > 0)
            whereClause += "') ";

        sqlquery += " " + sqlFrom;

        if (yrcnt > 0 || mtcnt > 0 || vlcnt > 0)
            sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();
        gvWeekly.DataSource = ds;
        gvWeekly.DataBind();

    }

    protected void gvWeekly_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Right;

            //e.Row.Cells[0].Width = 30;
            //e.Row.Cells[1].Width = 30;
            //e.Row.Cells[2].Width = 30;
            //e.Row.Cells[3].Width = 50;
            //e.Row.Cells[4].Width = 50;
            //e.Row.Cells[5].Width = 50;
            //e.Row.Cells[6].Width = 50;
            //e.Row.Cells[7].Width = 50;
            //e.Row.Cells[8].Width = 50;
            //e.Row.Cells[9].Width = 50;
            //e.Row.Cells[10].Width = 50;
            //e.Row.Cells[11].Width = 70;
            //e.Row.Cells[12].Width = 70;
            //e.Row.Cells[13].Width = 70;
            //e.Row.Cells[14].Width = 70;
            //e.Row.Cells[15].Width = 140;

            e.Row.Cells[4].Text = Double.Parse(e.Row.Cells[4].Text.ToString()).ToString("0.00");
            e.Row.Cells[5].Text = Double.Parse(e.Row.Cells[5].Text.ToString()).ToString("0.00");
            e.Row.Cells[6].Text = Double.Parse(e.Row.Cells[6].Text.ToString()).ToString("0.00");
            e.Row.Cells[7].Text = Double.Parse(e.Row.Cells[7].Text.ToString()).ToString("0.00");
            e.Row.Cells[8].Text = Double.Parse(e.Row.Cells[8].Text.ToString()).ToString("0.00");
            e.Row.Cells[9].Text = Double.Parse(e.Row.Cells[9].Text.ToString()).ToString("0.00");
            //e.Row.Cells[10].Text = Double.Parse(e.Row.Cells[10].Text.ToString()).ToString("0.00");
        }
    }

    protected void btnSearchMonthly_Click(object sender, EventArgs e)
    {
        gvDaily.Visible = false;
        gvWeekly.Visible = false;
        gvMonthly.Visible = true;

        dataConn.Open();
        string sqlquery = "select  DateYear as Year, DateMonth as Month, VehicleId as Vehicle, HoursRunning as HrsRun, HoursIdle as HrsIdle, HoursOff as HrsOff,  round(TotalMiles,2) as Miles, AvgSpeed, round(MaxSpeed,2) as MaxSpeed ";
        string sqlFrom = "  from dbo.StrataGIS_Mart_Monthly_Report ";
        string sqlOrder = " order by DateYear, DateMonth, VehicleId";
        string whereClause = " where ";
        int yrcnt = 0;
        foreach (ListItem i in cklYear.Items)
        {
            if (i.Selected == true)
            {
                if (yrcnt == 0)
                {
                    whereClause += "DateYear in (" + i.Text;
                }
                else
                {
                    whereClause += "," + i.Text;
                }
                yrcnt += 1;
            }
        }
        if (yrcnt > 0)
            whereClause += ") ";

        int mtcnt = 0;
        foreach (ListItem i in cklMonth.Items)
        {
            if (i.Selected == true)
            {
                if (mtcnt == 0)
                {
                    if (yrcnt > 0)
                        whereClause += " and ";
                    whereClause += " DateMonth in (" + i.Text;
                }
                else
                {
                    whereClause += "," + i.Text;
                }
                mtcnt += 1;
            }
        }
        if (mtcnt > 0)
            whereClause += ") ";

        int vlcnt = 0;
        foreach (ListItem i in cklVehicles.Items)
        {
            if (i.Selected == true)
            {
                if (vlcnt == 0)
                {
                    if (yrcnt > 0 || mtcnt > 0)
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
        if (vlcnt > 0)
            whereClause += "') ";

        sqlquery += " " + sqlFrom;

        if (yrcnt > 0 || mtcnt > 0 || vlcnt > 0)
            sqlquery += whereClause;
        sqlquery += " " + sqlOrder;
        SqlCommand command = new SqlCommand(sqlquery, dataConn);
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(command);
        sda.Fill(ds);

        dataConn.Close();
        gvMonthly.DataSource = ds;
        gvMonthly.DataBind();

    }

    protected void gvMonthly_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Right;

            //e.Row.Cells[0].Width = 30;
            //e.Row.Cells[1].Width = 30;
            //e.Row.Cells[2].Width = 50;
            //e.Row.Cells[3].Width = 50;
            //e.Row.Cells[4].Width = 50;
            //e.Row.Cells[5].Width = 50;
            //e.Row.Cells[6].Width = 50;
            //e.Row.Cells[7].Width = 60;
            //e.Row.Cells[8].Width = 60;
            //e.Row.Cells[9].Width = 60;
            //e.Row.Cells[10].Width = 70;
            //e.Row.Cells[11].Width = 70;
            //e.Row.Cells[12].Width = 70;
            //e.Row.Cells[13].Width = 70;
            //e.Row.Cells[14].Width = 160;

            e.Row.Cells[3].Text = Double.Parse(e.Row.Cells[3].Text.ToString()).ToString("0.00");
            e.Row.Cells[4].Text = Double.Parse(e.Row.Cells[4].Text.ToString()).ToString("0.00");
            e.Row.Cells[5].Text = Double.Parse(e.Row.Cells[5].Text.ToString()).ToString("0.00");
            e.Row.Cells[6].Text = Double.Parse(e.Row.Cells[6].Text.ToString()).ToString("0.00");
            e.Row.Cells[7].Text = Double.Parse(e.Row.Cells[7].Text.ToString()).ToString("0.00");
            e.Row.Cells[8].Text = Double.Parse(e.Row.Cells[8].Text.ToString()).ToString("0.00");
            //e.Row.Cells[9].Text = Double.Parse(e.Row.Cells[9].Text.ToString()).ToString("0.00");
        }
    }
}
