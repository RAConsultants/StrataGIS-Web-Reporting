using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;


public partial class Test: System.Web.UI.Page
{
    public double fttom = 0.30480060960121920243840487680975;
    public double RAD = 180.0 / Math.PI;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btcclick_Click(object sender, EventArgs e)
    {
        lblout.Text = "";
        lblout2.Text = "";
        lbltest.Text = "";
        lblout2.Text = spc(Convert.ToDouble(tbx.Text), Convert.ToDouble(tby.Text));

    }

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

        double[] spcc = spccKN;

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

        lblout.Text = "(LAT, LON) = (" + Latatude.ToString() + ", " + Longitude.ToString() + ")";
        var request = new Google.Maps.LatLng(Latatude, Longitude);
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
}