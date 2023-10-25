using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Device.Location;

namespace ESOperatorTaxi
{
    class Geopoint
    {
        private string _city;
        private string _street;
        private int _home;
        private GeoCoordinate _coordinate;

        public Geopoint(string city, string street, int home)
        {
            XDocument xdoc = GetXmlSearchResult(city, street, home);
            var place = xdoc.Element("searchresults").Element("place");
            string lat = place.Attribute("lat").Value;
            string lon = place.Attribute("lon").Value;
            double latitude = Convert.ToDouble(lat.Replace('.', ','));
            double longitude = Convert.ToDouble(lon.Replace('.', ','));
            _coordinate = new GeoCoordinate(latitude, longitude);

            _city = city;
            _street = street;
            _home = home;
        }

        public string City => _city;
        public string Street => _street;
        public int Home => _home;
        public GeoCoordinate Coordinate => _coordinate;

        private XDocument GetXmlSearchResult(string city, string street, int home)
        {
            string requestUri = string.Format(
                "https://nominatim.openstreetmap.org/search?city={0}&street={1}&format=xml",
                city, home + " " + street
                );

            XDocument xdoc = null;
            WebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.UserAgent = "Other";
                response = request.GetResponse();
                xdoc = XDocument.Load(response.GetResponseStream());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return xdoc;
        }

        public double GetDistanceTo(Geopoint geopoint)
        {
            return _coordinate.GetDistanceTo(geopoint._coordinate);
        }
    }
}
