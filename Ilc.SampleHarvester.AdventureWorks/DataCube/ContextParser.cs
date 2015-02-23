using Ilc.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ilc.SampleHarvester.AdventureWorks.DataCube
{
    /// <summary>
    /// This Parser identifies Metainformations and Contextinformations and trys to categorize them.
    /// </summary>
    public class ContextParser
    {
        private InformationContext context;

        public ContextParser(InformationContext context)
        {
            this.context = context;

            if(!ParseMeta(context))
                Parse(context);        
        }        

        /// <summary>
        /// Parses the context for known meta informations.
        /// </summary>
        /// <param name="context">The InformationContext to be parsed</param>
        /// <returns>Returns true if a known meta information was parsed, otherwise false.</returns>
        private bool ParseMeta(InformationContext context)
        {
            long numberBuffer;
            if (string.Equals(context.Meta, "Debitorennr", StringComparison.OrdinalIgnoreCase))
            {                
                if (long.TryParse(context.Context, out numberBuffer))
                {
                    DebitorNumber = numberBuffer;
                    return IsDebitorNumber = true;
                }
            }
            if (string.Equals(context.Meta, "Kreditornr", StringComparison.OrdinalIgnoreCase))
            {
                if (long.TryParse(context.Context, out numberBuffer))
                {
                    KreditorNumber = numberBuffer;
                    return IsKreditorNumber = true;
                }
            }
            if (string.Equals(context.Meta, "Email", StringComparison.OrdinalIgnoreCase))
            {
                Ilc.Linguistic.EmailAddress email;
                if (Ilc.Linguistic.EmailAddress.TryParse(context.Context, out email))
                {
                    EmailAddress = email;
                    return IsEmailAddress = true;
                }
            }
            return false;
        }

        /// <summary>
        /// Parses the context value for known formats and trys to categorize them. If no categorization could be done it defaults to companyname.
        /// </summary>
        /// <param name="context">The InformationContext to be parsed</param>
        private void Parse(InformationContext context)
        {
            long numberBuffer;
            Ilc.Linguistic.EmailAddress email;
            if (Ilc.Linguistic.EmailAddress.TryParse(context.Context, out email))
            {
                IsEmailAddress = true;
                EmailAddress = email;
            }
            else if (long.TryParse(context.Context, out numberBuffer))
            {
                IsDebitorNumber = IsKreditorNumber = true;
                DebitorNumber = KreditorNumber = numberBuffer;
            }
            else
            {
                IsCompanyName = true;
                CompanyName = context.Context;
            }
        }

        public long DebitorNumber { get; set; }

        public bool IsDebitorNumber { get; set; }

        public long KreditorNumber { get; set; }
        
        public bool IsKreditorNumber { get; set; }

        public Ilc.Linguistic.EmailAddress EmailAddress { get; set; }

        public bool IsEmailAddress { get; set; }

        public string CompanyName { get; set; }

        public bool IsCompanyName { get; set; }
    }
}