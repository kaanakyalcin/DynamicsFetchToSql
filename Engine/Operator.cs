using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Operator
    {
        public static Dictionary<string, string> GetList()
        {
            Dictionary<string, string> operatorReplacement = new Dictionary<string, string>();

            operatorReplacement.Add("eq", "=");
            operatorReplacement.Add("neq", "!=");
            operatorReplacement.Add("ne", "!=");
            operatorReplacement.Add("gt", ">");
            operatorReplacement.Add("ge", ">=");
            operatorReplacement.Add("le", "<=");
            operatorReplacement.Add("lt", "<");
            operatorReplacement.Add("like", "LIKE");
            operatorReplacement.Add("not-like", "NOT LIKE");
            operatorReplacement.Add("in", "in");
            operatorReplacement.Add("not-in", "not in");
            operatorReplacement.Add("between", "TBD");
            operatorReplacement.Add("not-between", "TBD");
            operatorReplacement.Add("null", "NULL");
            operatorReplacement.Add("not-null", "IS NOT NULL");
            //operatorReplacement.Add("yesterday", "BETWEEN CONVERT(DATE, DATEADD(DAY, -1, GETDATE())) AND CONVERT(DATE, GETDATE())");
            operatorReplacement.Add("yesterday", "BETWEEN CONVERT(DATE, DATEADD(DAY, -1, GETDATE())) AND DATEADD(S, -1, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))");            
            operatorReplacement.Add("today", "BETWEEN CONVERT(date, GETDATE()) and DATEADD(s, 24*60*60-1, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))");
            //operatorReplacement.Add("today", "= CONVERT(DATE, GETDATE())");
            //operatorReplacement.Add("tomorrow", "BETWEEN CONVERT(DATE, DATEADD(DAY, 1, GETDATE())) AND CONVERT(DATE, DATEADD(DAY, 2, GETDATE()))");
            operatorReplacement.Add("tomorrow", "BETWEEN CONVERT(DATE, DATEADD(DAY, 1, GETDATE())) AND DATEADD(S, 2*24*60*60-1, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))");
            operatorReplacement.Add("last-seven-days", "BETWEEN CONVERT(DATE, DATEADD(DAY, -7, GETDATE())) AND CONVERT(DATE, GETDATE())");
            operatorReplacement.Add("next-seven-days", "BETWEEN CONVERT(DATE, DATEADD(DAY, +1, GETDATE())) AND CONVERT(DATE, DATEADD(D, +9, GETDATE()))");
            operatorReplacement.Add("last-week", "BETWEEN dateadd(week, -1, dateadd(week, datediff(week, 0, getdate()), 0)) AND dateadd(week, datediff(week, 0, getdate()), 0) ");
            operatorReplacement.Add("this-week", "BETWEEN dateadd(week, datediff(week, 0, getdate()), 0) and dateadd(day, 7, dateadd(week, datediff(week, 0, getdate()), 0))");
            operatorReplacement.Add("next-week", "BETWEEN dateadd(day, 7, dateadd(week, datediff(week, 0, getdate()), 0)) and dateadd(day, 14, dateadd(week, datediff(week, 0, getdate()), 0))");
            operatorReplacement.Add("last-month", "BETWEEN DATEADD(month, DATEDIFF(month, -1, getdate()) - 2, 0) AND DATEADD(month, DATEDIFF(month, 0, getdate()), 0)");
            operatorReplacement.Add("this-month", "BETWEEN DATEADD(month, DATEDIFF(month, -1, getdate()) - 1, 0) AND DATEADD(month, DATEDIFF(month, -1, getdate()), 0) ");
            operatorReplacement.Add("next-month", "BETWEEN DATEADD(month, DATEDIFF(month, -1, getdate()), 0) AND DATEADD(MONTH, 1, DATEADD(month, DATEDIFF(month, -1, getdate()), 0))");
            operatorReplacement.Add("on", "BETWEEN CONVERT(DATE, '{0}') AND CONVERT(DATE, DATEADD(DAY, +1, '{0}'))");
            operatorReplacement.Add("on-or-before", "< dateadd(day, 1, dateadd(DAY, datediff(day, 0, '{0}'),0)) ");
            operatorReplacement.Add("on-or-after", ">= dateadd(DAY, datediff(day, 0, '{0}'),0) ");
            operatorReplacement.Add("last-year", "BETWEEN DATEADD(YEAR, -1, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)");
            operatorReplacement.Add("this-year", "BETWEEN DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND DATEADD(YEAR, 1, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("next-year", "BETWEEN DATEADD(YEAR, 1, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(YEAR, 2, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("last-x-hours", "BETWEEN dateadd(Hour, {0}, getdate()) and getdate()");
            operatorReplacement.Add("next-x-hours", "BETWEEN getdate() and dateadd(Hour, {0}, getdate())");
            operatorReplacement.Add("last-x-days", "between dateadd(day, {0}, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()),0)) ");
            operatorReplacement.Add("next-x-days", "between dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(day, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("last-x-weeks", "between dateadd(week, {0}, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()),0)) ");
            operatorReplacement.Add("next-x-weeks", "between dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(week, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("last-x-months", "between dateadd(Month, {0}, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()),0)) ");
            operatorReplacement.Add("next-x-months", "between dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(Month, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("olderthan-x-months", "< dateadd(month, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("olderthan-x-years", "< dateadd(year, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("olderthan-x-weeks", "< dateadd(week, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("olderthan-x-days", "< dateadd(day, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("olderthan-x-hours", "< dateadd(hour, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("olderthan-x-minutes", "< dateadd(minute, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("last-x-years", "between dateadd(YEAR, {0}, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()), 0))");
            operatorReplacement.Add("next-x-years", "between dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()), 0)) and dateadd(YEAR, {0}, dateadd(DAY, datediff(day, 0, getdate()),0))");
            operatorReplacement.Add("eq-userid", "=");
            operatorReplacement.Add("ne-userid", "!=");
            /*Cancel
            operatorReplacement.Add("eq-userteams", "TBD");
            operatorReplacement.Add("eq-useroruserteams", "TBD");
            operatorReplacement.Add("eq-useroruserhierarchy", "TBD");
            operatorReplacement.Add("eq-useroruserhierarchyandteams", "TBD");
            operatorReplacement.Add("eq-businessid", "TBD");
            operatorReplacement.Add("ne-businessid", "TBD");
            operatorReplacement.Add("eq-userlanguage", "TBD");
            operatorReplacement.Add("under", "TBD");
            operatorReplacement.Add("eq-or-under", "TBD");
            operatorReplacement.Add("not-under", "TBD");
            operatorReplacement.Add("above", "TBD");
            operatorReplacement.Add("eq-or-above", "TBD");
            Cancel*/
            operatorReplacement.Add("this-fiscal-year", "BETWEEN DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND DATEADD(YEAR, 1, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("this-fiscal-period", "BETWEEN DATEADD(MONTH, ((MONTH(GETDATE()) - 1) / 3) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(MONTH, ((MONTH(GETDATE()) - 1) / 3 + 1) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("next-fiscal-year", "BETWEEN DATEADD(YEAR, 1, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(YEAR, 2, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("next-fiscal-period", "BETWEEN DATEADD(MONTH, (((MONTH(GETDATE()) - 1) / 3) + 1) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(MONTH, (((MONTH(GETDATE()) - 1) / 3 + 1) + 1) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("last-fiscal-year", "BETWEEN DATEADD(YEAR, -1, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)");
            operatorReplacement.Add("last-fiscal-period", "BETWEEN DATEADD(MONTH, (((MONTH(GETDATE()) - 1) / 3) - 1) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(MONTH, (((MONTH(GETDATE()) - 1) / 3 + 1) - 1) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("last-x-fiscal-years", "between dateadd(YEAR, {0}, dateadd(DAY, datediff(day, 0, getdate()),0)) and dateadd(day, 1, dateadd(DAY, datediff(day, 0, getdate()), 0))");
            operatorReplacement.Add("last-x-fiscal-periods", "BETWEEN DATEADD(MONTH, (((MONTH(GETDATE()) - 1) / 3)  {0}) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(MONTH, ((MONTH(GETDATE()) - 1) / 3) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("next-x-fiscal-years", "BETWEEN DATEADD(YEAR, 1, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(YEAR, 1 {0}, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("next-x-fiscal-periods", "BETWEEN DATEADD(MONTH, (((MONTH(GETDATE()) - 1) / 3) + 1) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(MONTH, (((MONTH(GETDATE()) - 1) / 3) + 1 {0}) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("in-fiscal-year", "BETWEEN DATEADD(yy, DATEDIFF(yy, 0, '{0}0101'), 0) AND DATEADD(YEAR, 1, DATEADD(yy, DATEDIFF(yy, 0, '{0}0101'), 0))");
            operatorReplacement.Add("in-fiscal-period", "BETWEEN DATEADD(MONTH, ({0}-1) * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0)) AND DATEADD(MONTH, {0} * 3, DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0))");
            operatorReplacement.Add("in-fiscal-period-and-year", "TBD");
            operatorReplacement.Add("in-or-before-fiscal-period-and-year", "TBD");
            operatorReplacement.Add("in-or-after-fiscal-period-and-year", "TBD");
            operatorReplacement.Add("begins-with", "= %");
            operatorReplacement.Add("not-begin-with", "!= %");
            operatorReplacement.Add("ends-with", "=");
            operatorReplacement.Add("not-end-with", "!=");
            

            return operatorReplacement;
        }

        public static List<string> GetInStatements()
        {
            List<string> inStatements = new List<string>();

            inStatements.Add("in");
            inStatements.Add("not-in");
            inStatements.Add("not in");

            return inStatements;
        }

        public static List<string> GetxMinusStatements()
        {
            List<string> xMinusStatements = new List<string>();

            xMinusStatements.Add("last-x-hours");
            xMinusStatements.Add("last-x-days");
            xMinusStatements.Add("last-x-weeks");
            xMinusStatements.Add("last-x-months");
            xMinusStatements.Add("last-x-years");
            xMinusStatements.Add("olderthan-x-months");
            xMinusStatements.Add("olderthan-x-years");
            xMinusStatements.Add("olderthan-x-weeks");
            xMinusStatements.Add("olderthan-x-days");
            xMinusStatements.Add("olderthan-x-hours");
            xMinusStatements.Add("olderthan-x-minutes");
            xMinusStatements.Add("last-x-fiscal-years");
            xMinusStatements.Add("last-x-fiscal-periods");
            xMinusStatements.Add("last-x-fiscal-periods");

            return xMinusStatements;
        }

        public static List<string> GetxPlusStatements()
        {
            List<string> xPlusStatements = new List<string>();

            xPlusStatements.Add("next-x-hours");
            xPlusStatements.Add("next-x-days");
            xPlusStatements.Add("next-x-weeks");
            xPlusStatements.Add("next-x-months");
            xPlusStatements.Add("next-x-years");
            xPlusStatements.Add("next-x-fiscal-periods");
            xPlusStatements.Add("next-x-fiscal-years");

            return xPlusStatements;
        }

        public static List<string> GetInFiscalStatements()
        {
            List<string> inFiscalYearStatements = new List<string>();

            inFiscalYearStatements.Add("in-fiscal-year");
            inFiscalYearStatements.Add("in-fiscal-period");
            inFiscalYearStatements.Add("on-or-after");
            inFiscalYearStatements.Add("on-or-before");
            inFiscalYearStatements.Add("on");

            return inFiscalYearStatements;
        }
    }
}