using System;
using ServiceStack;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BauerCircles.Shared
{

	/// <summary>
	/// RequestDTO used by ServiceStack
	/// </summary>
	[Route("colors/random")]
	public class NewRandomColorRequest : IReturn<List<RandomColorData>>
	{
		public string Format { get; set; }
	}

	/// <summary>
	/// ResponseDTO used by ServiceStack from http://www.colourlovers.com/api/
	/// Only the fields of interest for the app are avaible here 
	/// </summary>
	public class RandomColorData
	{
		/// <summary>
		/// Id of the Colour
		/// </summary>
		/// <value>The identifier.</value>
		public int Id { get; set; }
		/// <summary>
		/// Name of the Color
		/// </summary>
		/// <value>The title.</value>
		public string Title { get; set; }
		/// <summary>
		/// Hex represenation of the Colour
		/// </summary>
		/// <value>The hex.</value>
		public string Hex { get; set; }
		/// <summary>
		/// RGB representation of the colour
		/// </summary>
		/// <value>The rgb.</value>
		public Rgb Rgb { get; set; }
	}

	public class Rgb
	{
		public int Red { get; set; }
		public int Green { get; set; }
		public int Blue { get; set; }
	}


	/// <summary>
	/// Simple REST client to talk with CoulourLovers
	/// </summary>
	public class ColourLoverClient
	{
		public static string BaseURL = "http://www.colourlovers.com/api/";

		public static async Task<RandomColorData> GetNewRandomColorAsync()
		{
			var res = await new JsonServiceClient (BaseURL)
				.GetAsync (new NewRandomColorRequest (){Format = "json"});
			return res.First ();
		}
	}
}

