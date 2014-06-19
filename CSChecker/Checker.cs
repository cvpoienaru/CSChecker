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
	/// Provides methods and properties for the effective testing of the units.
	/// This class cannot be inherited.
	/// </summary>
	public sealed class Checker
	{
		#region *** Fields ***
		/// <summary>
		/// The path to the current output folder.
		/// </summary>
		private string path;

		/// <summary>
		/// A collection of all the units composing the current session of the checker.
		/// </summary>
		private Queue<Unit> unitCollection;
		#endregion *** Fields ***



		#region *** Constructors ***
		/// <summary>
		/// Initializes a new instance of <see cref="CSChecker.Checker"/> class.
		/// </summary>
		/// 
		/// <param name="path">The path to the current output folder.</param>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the path argument is null, empty or contains only white spaces.
		/// </exception>
		public Checker (string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentException("Invalid path.");

			this.path = path;
			this.unitCollection = new Queue<Unit>(0);
		}
		#endregion *** Constructors ***



		#region *** Methods ***
		/// <summary>
		/// Adds a new unit to the collection.
		/// </summary>
		/// 
		/// <param name="unit">The unit to be added to the collection.</param>
		/// 
		/// <exception cref="System.ArgumentNullException">
		/// Exception thrown when the unit argument is null.
		/// </exception>
		public void AddUnit (Unit unit)
		{
			if (unit == null)
				throw new ArgumentNullException("Unit argument is null.");

			this.unitCollection.Enqueue(unit);
		}

		/// <summary>
		/// Creates, if necessary, the output folder for the current unit as well as the output file.
		/// </summary>
		/// 
		/// <param name="description">The description of the current unit.</param>
		/// <param name="overwrite">
		/// True if the summary of the current unit should be written in separate files at each new
		/// execution of the checker, false otherwise.</param>
		/// 
		/// <returns>Returns the exact path of the file used for writing the summary.</returns>
		private string PrepareFile (string description, bool overwrite)
		{
			string directoryPath = this.path + Path.DirectorySeparatorChar + description;
			string filePath = directoryPath + Path.DirectorySeparatorChar + description;

			// If the directory for the current unit does not exist, create it.
			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);

			// If the original file for the current unit does not exist, create it and use it for writing
			// the summary.
			if (!File.Exists(filePath + ".txt"))
			{
				filePath += ".txt";
				using (File.Create(filePath)) { }
				return filePath;
			}

			// If the original file for the current unit exists, check if we should overwrite it.
			// If this is the case, use the original file for writing.
			if (overwrite)
				return filePath + ".txt";

			// Otherwise, create a new file using a correct indexer in order to exist no conflicts between
			// earlier output files and the current one.
			int i = 0;
			while (File.Exists(filePath + "(" + (++i) + ").txt")) { }

			filePath += "(" + i + ").txt";
			using (File.Create(filePath)) { }
			return filePath;
		}

		/// <summary>
		/// Runs the collection of units and prints the summary to the output file corresponding to each
		/// unit.
		/// </summary>
		public void Run ()
		{
			for (int i = 0; i < this.unitCollection.Count; i++)
			{
				// Dequeue a unit from the collection.
				Unit unit = this.unitCollection.Dequeue();

				// Run the unit and print the summary to the corresponding output file.
				unit.Run();
				Printer.PrintSummary(unit.ToString(), this.PrepareFile(unit.Description, false));

				// Enqueue it back in the collection.
				this.unitCollection.Enqueue(unit);
			}
		}
		#endregion *** Methods ***
	}
}