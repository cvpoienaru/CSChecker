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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSChecker.Utilities;

namespace CSChecker.Definitions
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Unit
	{
		#region *** Fields ***
		/// <summary>
		/// 
		/// </summary>
		private const int DefaultWidth = 80;

		/// <summary>
		/// 
		/// </summary>
		private Stopwatch watch;

		/// <summary>
		/// 
		/// </summary>
		private Queue<Test> testCollection;

		/// <summary>
		/// 
		/// </summary>
		private string description;

		/// <summary>
		/// 
		/// </summary>
		private int width;

		/// <summary>
		/// 
		/// </summary>
		private int passed;

		/// <summary>
		/// 
		/// </summary>
		private int total;
		#endregion *** Fields ***



		#region *** Constructors ***
		/// <summary>
		/// 
		/// </summary>
		/// 
		/// <param name="description"></param>
		/// <param name="width"></param>
		public Unit (string description, int width)
		{
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Invalid description.");

			if (width < Unit.DefaultWidth)
				throw new ArgumentException("The specified width is less than the minimum valid width.");

			this.description = description;
			this.width = width;
			this.testCollection = new Queue<Test>(0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// 
		/// <param name="description"></param>
		public Unit (string description)
			: this(description, Unit.DefaultWidth)
		{

		}
		#endregion *** Constructors ***



		#region *** Properties ***
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			get { return this.description; }
		}
		#endregion *** Properties ***



		#region *** Methods ***
		/// <summary>
		/// 
		/// </summary>
		/// 
		/// <param name="test"></param>
		protected void AddTest (Test test)
		{
			if (test == null)
				throw new ArgumentNullException("");

			this.testCollection.Enqueue(test);
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Run ()
		{
			this.watch = new Stopwatch();

			this.watch.Start();
			for (int i = 0; i < this.testCollection.Count; i++)
			{
				// Dequeue a test from the collection and run it.
				Test test = this.testCollection.Dequeue();
				test.Run();

				this.passed += test.Passed;
				this.total += test.Total;

				// Enqueue it back in the collection.
				this.testCollection.Enqueue(test);
			}
			this.watch.Stop();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString ()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder auxiliary = new StringBuilder();
			byte[] summary;

			auxiliary.AppendFormat("{0} ({1})", this.description.ToUpper(), DateTime.Now.ToString());
			stringBuilder.AppendLine(Printer.PrintCharacter('=', this.width));
			stringBuilder.AppendFormat(
				"{0}{1}",
				Printer.PrintCharacter(' ', (this.width - auxiliary.ToString().Length) / 2),
				auxiliary.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(Printer.PrintCharacter('=', this.width));

			for (int i = 0; i < this.testCollection.Count; i++)
			{
				Test test = this.testCollection.Dequeue();
				stringBuilder.AppendFormat("Test #{0} - {1}", i + 1, test.ToString());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(Printer.PrintCharacter('=', this.width));
				this.testCollection.Enqueue(test);
			}

			stringBuilder.AppendFormat(
				"Average Execution Time : {0} ms",
				Math.Round(((double)this.watch.ElapsedMilliseconds) / this.testCollection.Count, 2));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat(
				"Total Execution Time   : {0} ms",
				this.watch.ElapsedMilliseconds);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat(
				"Total Passed           : {0}/{1} => {2} %",
				this.passed,
				this.total,
				Math.Round(((double)(this.passed * 100)) / this.total, 2));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(Printer.PrintCharacter('=', this.width));

			summary = MessageDigest.Compute(
						Encoding.UTF8.GetBytes(stringBuilder.ToString()),
						MessageDigestOutputSize.Bits256);
			auxiliary.Clear();
			foreach (byte b in summary)
			{
				auxiliary.AppendFormat("{0:x2}", b);
			}

			stringBuilder.AppendFormat("UNIT DIGEST: {0}", auxiliary.ToString());
			stringBuilder.AppendLine();
			stringBuilder.Append(Printer.PrintCharacter('=', this.width));

			try
			{
				return stringBuilder.ToString();
			}
			finally
			{
				auxiliary.Clear();
				stringBuilder.Clear();
			}
		}
		#endregion *** Methods ***
	}
}