/**************************************************************************************************
 **************************************************************************************************
 **************************************************************************************************
 ***																							***
 ***		Author:		Codrin-Victor Poienaru													***
 ***		Email:		cvpoienaru@gmail.com													***
 ***																							***
 ***		Copyright (c) 2014, Codrin-Victor Poienaru.											***
 ***		All rights reserved.																***
 ***																							***
 ***	Redistribution and use in source and binary forms, with or without modification,		***
 ***	are permitted provided that the following conditions are met:							***
 ***																							***
 ***		* Redistributions of source code must retain the above copyright notice, this		***
 ***		list of conditions and the following disclaimer.									***
 ***																							***
 ***		* Redistributions in binary form must reproduce the above copyright notice, this	***
 ***		list of conditions and the following disclaimer in the documentation and/or			***
 ***		other materials provided with the distribution.										***
 ***																							***
 ***	This software is provided by the copyright holders and contributors "as is" and any		***
 ***	express or implied warranties, including, but not limited to, the implied warranties	***
 ***	of merchantability and fitness for a particular purpose are disclaimed. In no event		***
 ***	shall the copyright holder or contributors be liable for any direct, indirect,			***
 ***	incidental, special, exemplary, or consequential damages (including, but not limited	***
 ***	to, procurement of substitute goods or services; loss of use, data, or profits; or		***
 ***	business interruption) however caused and on any theory of liability, whether in		***
 ***	contract, strict liability, or tort (including negligence or otherwise) arising in		***
 ***	any way out of the use of this software, even if advised of the possibility of such		***
 ***	damage.																					***
 ***																							***
 **************************************************************************************************
 **************************************************************************************************
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSChecker.Definitions;
using CSChecker.Utilities;

namespace CSChecker
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Checker
	{
		#region *** Fields ***
		/// <summary>
		/// 
		/// </summary>
		private string path;

		/// <summary>
		/// 
		/// </summary>
		private Queue<Unit> unitCollection;
		#endregion *** Fields ***



		#region *** Constructors ***
		/// <summary>
		/// 
		/// </summary>
		/// 
		/// <param name="path"></param>
		public Checker (string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentException("");

			this.path = path;
			this.unitCollection = new Queue<Unit>(0);
		}
		#endregion *** Constructors ***



		#region *** Methods ***
		/// <summary>
		/// 
		/// </summary>
		/// 
		/// <param name="unit"></param>
		public void AddUnit (Unit unit)
		{
			if (unit == null)
				throw new ArgumentNullException("Unit argument is null.");

			this.unitCollection.Enqueue(unit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// 
		/// <param name="description"></param>
		/// <param name="overwrite"></param>
		/// 
		/// <returns></returns>
		private string PrepareFile (string description, bool overwrite)
		{
			string directoryPath = this.path + Path.DirectorySeparatorChar + description;
			string filePath = directoryPath + Path.DirectorySeparatorChar + description;

			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);

			if (!File.Exists(filePath + ".txt"))
			{
				filePath += ".txt";
				using (File.Create(filePath)) { }
				return filePath;
			}

			if (overwrite)
				return filePath + ".txt";

			int i = 0;
			while (File.Exists(filePath + "(" + (++i) + ").txt")) { }

			filePath += "(" + i + ").txt";
			using (File.Create(filePath)) { }
			return filePath;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run ()
		{
			for (int i = 0; i < this.unitCollection.Count; i++)
			{
				Unit unit = this.unitCollection.Dequeue();

				unit.Run();
				Printer.PrintSummary(unit.ToString(), this.PrepareFile(unit.Description, false));

				this.unitCollection.Enqueue(unit);
			}
		}
		#endregion *** Methods ***
	}
}