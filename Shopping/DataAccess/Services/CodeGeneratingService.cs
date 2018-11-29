using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entity;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Services
{
    public class CodeGeneratingService : ICodeGeneratingService
    {
        private readonly ShoppingContext _shoppingContext;

        public CodeGeneratingService(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public string GenerateCode(string prefix)
        {
            var codeGenerating = _shoppingContext.CodeGeneratings.FirstOrDefault(x =>
                x.Prefix.Equals(prefix, StringComparison.CurrentCultureIgnoreCase));
            if (codeGenerating != null)
            {
                var currentDate = DateTime.Now.Date;
                if (currentDate.Date.Equals(codeGenerating.LastGeneratedDateTime.Date))
                {
                    codeGenerating.GeneratingNumber++;
                }
                else
                {
                    codeGenerating.GeneratingNumber = 0;
                    codeGenerating.LastGeneratedDateTime = currentDate;
                }
                UpdateGenerateCode(codeGenerating);
            }
            else
            {
                codeGenerating = new CodeGenerating()
                {
                    GeneratingNumber = 0,
                    LastGeneratedDateTime = DateTime.Now.Date,
                    Prefix = prefix
                };
                InsertGenerateCode(codeGenerating);
            }

            var res = prefix + codeGenerating.LastGeneratedDateTime.ToString("_yyyyMMdd_") + codeGenerating.GeneratingNumber.ToString();
            return res;
        }

        private void InsertGenerateCode(CodeGenerating codeGenerating)
        {
            _shoppingContext.CodeGeneratings.Add(codeGenerating);
            _shoppingContext.SaveChanges();
        }

        private void UpdateGenerateCode(CodeGenerating codeGenerating)
        {
            _shoppingContext.SaveChanges();
        }
    }
}
