using Dividendos.B3.Config;
using Dividendos.B3.Interface;
using Dividendos.B3.Interface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using K.Logger;
using System.Threading;

namespace Dividendos.B3
{
    public class ImportB3Helper : IImportB3Helper
    {
        string _workingDirectoryCei;
        string _workingDirectoryAnacondaActivate;

        public ImportB3Helper(string workingDirectoryCei, string workingDirectoryAnacondaActivate)
        {
            _workingDirectoryCei = workingDirectoryCei;
            _workingDirectoryAnacondaActivate = workingDirectoryAnacondaActivate;
        }


        //private CancellationTokenSource TokenSource { get; set; }

        public ImportCeiResult ImportCei(string username, string password, string idUser, bool automaticProcess, string startDateFilter, CancellationTokenSource cancellationTokenSource = null)
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();

            ImportCeiResult objImportCeiResult = new ImportCeiResult();
            objImportCeiResult.Identifier = username;
            objImportCeiResult.Password = password;
            objImportCeiResult.IdUser = idUser;
            objImportCeiResult.AutomaticProcess = automaticProcess;

            List<StockOperation> lstStockPortfolio = new List<StockOperation>();
            List<StockOperation> lstStockOperation = new List<StockOperation>();
            List<DividendImport> lstDividend = new List<DividendImport>();
            List<TesouroDiretoImport> lstTesouroDireto = new List<TesouroDiretoImport>();
            List<GenericImport> lstGenericImport = new List<GenericImport>();
            List<StockOperation> lstStockAveragePrice = new List<StockOperation>();

            objImportCeiResult.Json = GetGeneric(ref lstGenericImport, username, password, startDateFilter, cancellationTokenSource);
            //GetFakeResult(ref lstGenericImport);

            //stopWatch.Stop();
            //// Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = stopWatch.Elapsed;

            //// Format and display the TimeSpan value.
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);
            //Console.WriteLine("RunTime " + elapsedTime);

            if ((lstGenericImport != null && lstGenericImport.Count > 0))
            {
                List<GenericImport> lstError = lstGenericImport.Where(objGenTmp => objGenTmp.ImportType == 99 ||
                                                                                   objGenTmp.ImportType == 100 ||
                                                                                   objGenTmp.ImportType == 101 ||
                                                                                   objGenTmp.ImportType == 102 ||
                                                                                   objGenTmp.ImportType == 104 ||
                                                                                   objGenTmp.ImportType == 105).ToList();


                if ((lstError != null && lstError.Count > 0))
                {
                    objImportCeiResult.Imported = false;
                    string message = lstError[0].Broker;
                    int importType = lstError[0].ImportType;
                    objImportCeiResult.ErrorCode = importType;

                    if (importType == 99)
                    {
                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("consultados"))
                        {
                            objImportCeiResult.Message = "Usuário não encontrado";
                        }
                        else if (!string.IsNullOrWhiteSpace(message) && message.Contains("bloqueado"))
                        {
                            objImportCeiResult.UserBlocked = true;
                            objImportCeiResult.Message = "Sua senha no CEI está expirada. Solicite uma nova senha no site do CEI, após isso troque sua senha no app Dividendos.me. MUITA ATENÇÃO! Durante o período de transição do CEI, utilize o portal provisório CEI APP (clique no botão IR para ser direcionado para o site correto e realizar a alteração)";
                        }
                        else if (!string.IsNullOrWhiteSpace(message) && message.Contains("provisória"))
                        {
                            objImportCeiResult.UserBlocked = true;
                            objImportCeiResult.Message = "A senha do CEI informada é uma senha provisória. Você precisa acessar o portal CEI, escolher uma nova senha, e depois disso atualizar a senha no app Dividendos.me. MUITA ATENÇÃO! Durante o período de transição do CEI, utilize o portal provisório CEI APP (clique no botão IR para ser direcionado para o site correto e realizar a alteração)";
                        }
                        else
                        {
                            objImportCeiResult.UserBlocked = true;
                            objImportCeiResult.Message = "Senha Inválida! Valide sua senha no portal CEI e tente novamente. MUITA ATENÇÃO! Durante o período de transição do CEI, utilize o portal provisório CEI APP (clique no botão IR para ser direcionado para o site correto e realizar a alteração)";
                        }
                    }
                    else if (importType == 105 || importType == 102)
                    {
                        objImportCeiResult.ErrorCEI = true;
                        objImportCeiResult.Message = "Ocorreu uma falha durante a integração ou seus ativos ainda não estão no CEI (B3). Por favor, tente novamente em alguns minutos.";
                    }
                    else
                    {
                        objImportCeiResult.Message = "O site do CEI está inacessível agora. Por favor, tente novamente mais tarde.";
                    }
                }
                else
                {
                    List<GenericImport> lstGenericDividend = lstGenericImport.Where(objGenTmp => objGenTmp.ImportType == 1).ToList();


                    if ((lstGenericImport != null && lstGenericImport.Count > 0))
                    {
                        lstGenericDividend.ForEach(genericDividend =>
                        {
                            DividendImport dividendImport = new DividendImport();

                            decimal netValue = 0;
                            decimal grossValue = 0;
                            decimal.TryParse(genericDividend.NetVal, NumberStyles.Currency, new CultureInfo("pt-br"), out netValue);
                            decimal.TryParse(genericDividend.GrossVal, NumberStyles.Currency, new CultureInfo("pt-br"), out grossValue);

                            DateTime paymentDate;

                            dividendImport.Broker = GetBroker(genericDividend.Broker);
                            dividendImport.DividendType = genericDividend.DividendType;
                            dividendImport.NetValue = netValue;
                            dividendImport.GrossValue = grossValue;
                            dividendImport.BaseQuantity = Convert.ToInt32(genericDividend.BaseQtty.Replace(".", string.Empty).Replace(",", string.Empty));
                            dividendImport.Symbol = RemoveFractionalLetter(genericDividend.Symbol);

                            if (DateTime.TryParse(genericDividend.PaymentDt, new CultureInfo("pt-br"), DateTimeStyles.None, out paymentDate))
                            {
                                dividendImport.PaymentDate = paymentDate;
                            }

                            if (dividendImport.PaymentDate <= SqlDateTime.MinValue.Value)
                            {
                                dividendImport.PaymentDate = null;
                            }

                            lstDividend.Add(dividendImport);

                        });
                    }

                    List<GenericImport> lstGenericNegotiation = lstGenericImport.Where(objGenTmp => objGenTmp.ImportType == 2).ToList();
                    List<GenericImport> lstGenericPortfolio = lstGenericImport.Where(objGenTmp => objGenTmp.ImportType == 3).ToList();
                    List<GenericImport> lstGenericRent = lstGenericImport.Where(objGenTmp => objGenTmp.ImportType == 4).ToList();
                    List<GenericImport> lstGenericTesouroDireto = lstGenericImport.Where(objGenTmp => objGenTmp.ImportType == 5).ToList();
                    List<GenericImport> lstGenericAveragePrice = lstGenericImport.Where(objGenTmp => objGenTmp.ImportType == 6).ToList();

                    if (lstGenericTesouroDireto != null && lstGenericTesouroDireto.Count > 0)
                    {
                        lstGenericTesouroDireto.ForEach(genericRent =>
                        {
                            TesouroDiretoImport tesouroDiretoImport = new TesouroDiretoImport();

                            decimal netValue = 0;

                            decimal.TryParse(genericRent.NetVal, NumberStyles.Currency, new CultureInfo("pt-br"), out netValue);

                            tesouroDiretoImport.Broker = GetBroker(genericRent.Broker);
                            tesouroDiretoImport.Symbol = genericRent.Symbol;
                            tesouroDiretoImport.NetValue = netValue;

                            lstTesouroDireto.Add(tesouroDiretoImport);
                        });
                    }

                    if (lstGenericRent != null && lstGenericRent.Count > 0)
                    {
                        lstGenericRent.ForEach(genericRent =>
                        {
                            StockOperation stockPortfolio = new StockOperation();

                            stockPortfolio.Broker = GetBroker(genericRent.Broker);
                            stockPortfolio.AveragePrice = 0;
                            stockPortfolio.Symbol = RemoveFractionalLetter(genericRent.Symbol);
                            stockPortfolio.NumberOfShares = Convert.ToInt64(genericRent.NumShares.Replace(".", string.Empty).Replace(",", string.Empty));

                            bool foundRent = false;

                            if (lstGenericPortfolio != null && lstGenericPortfolio.Count > 0)
                            {
                                foundRent = lstGenericPortfolio.Exists(genPort => RemoveFractionalLetter(genPort.Symbol) == stockPortfolio.Symbol && !string.IsNullOrWhiteSpace(genPort.GridType) && genPort.GridType.Contains("EMPRESTIMO"));
                            }

                            if (foundRent)
                            {
                                objImportCeiResult.HasRent = true;
                            }
                            else
                            { 
                                DateTime expire;

                                if (DateTime.TryParse(genericRent.Expire, new CultureInfo("pt-br"), DateTimeStyles.None, out expire))
                                {
                                    if (expire >= DateTime.Now.Date)
                                    {
                                        lstStockPortfolio.Add(stockPortfolio);
                                    }
                                }
                            }
                        });
                    }

                    List<StockOperation> checkList = new List<StockOperation>();
                    ConvertStockOperation(checkList, lstGenericNegotiation);

                    if ((lstGenericPortfolio != null && lstGenericPortfolio.Count > 0) || (CheckNewStocksToPortfolio(checkList)))
                    {
                        lstGenericPortfolio.ForEach(genericPortfolio =>
                        {
                            StockOperation stockPortfolio = new StockOperation();

                            stockPortfolio.Broker = GetBroker(genericPortfolio.Broker);
                            stockPortfolio.AveragePrice = 0;
                            stockPortfolio.Symbol = RemoveFractionalLetter(genericPortfolio.Symbol);
                            stockPortfolio.NumberOfShares = Convert.ToInt64(genericPortfolio.NumShares.Replace(".", string.Empty).Replace(",", string.Empty));

                            lstStockPortfolio.Add(stockPortfolio);
                        });

                        ConvertStockOperation(lstStockOperation, lstGenericNegotiation);
                    }


                    if (lstGenericAveragePrice != null && lstGenericAveragePrice.Count > 0)
                    {
                        lstGenericAveragePrice.ForEach(genericPortfolio =>
                        {
                            decimal avgPrice = 0;
                            decimal.TryParse(genericPortfolio.AvgPrice, NumberStyles.Currency, new CultureInfo("pt-br"), out avgPrice);

                            StockOperation stockPortfolio = new StockOperation();

                            stockPortfolio.Broker = GetBroker(genericPortfolio.Broker);
                            stockPortfolio.AveragePrice = avgPrice;
                            stockPortfolio.Symbol = RemoveFractionalLetter(genericPortfolio.Symbol);
                            stockPortfolio.NumberOfShares = Convert.ToInt64(genericPortfolio.NumShares.Replace(".", string.Empty).Replace(",", string.Empty));
                            stockPortfolio.NumberOfSellShares = Convert.ToInt64(genericPortfolio.StockSpec.Replace(".", string.Empty).Replace(",", string.Empty));

                            lstStockAveragePrice.Add(stockPortfolio);
                        });
                    }

                    if (lstStockOperation.Count.Equals(0) &&
                        lstStockPortfolio.Count.Equals(0) &&
                        lstTesouroDireto.Count.Equals(0))
                    {
                        objImportCeiResult.Imported = false;
                        if (string.IsNullOrWhiteSpace(objImportCeiResult.Message))
                        {
                            objImportCeiResult.Message = "Ocorreu uma falha durante a integração ou seus ativos ainda não estão no CEI (B3). Por favor, tente novamente em alguns minutos.";
                        }
                    }
                    else
                    {
                        objImportCeiResult.Imported = true;
                    }
                }
            }
            else
            {
                objImportCeiResult.Imported = false;
                objImportCeiResult.ErrorCEI = true;
                objImportCeiResult.Message = "Ocorreu uma falha durante a integração ou seus ativos ainda não estão no CEI (B3). Por favor, tente novamente em alguns minutos.";
            }

            if (lstStockOperation.Count.Equals(0) &&
                lstStockPortfolio.Count.Equals(0) &&
                lstTesouroDireto.Count.Equals(0))
            {
                objImportCeiResult.Imported = false;
                if (string.IsNullOrWhiteSpace(objImportCeiResult.Message))
                {
                    objImportCeiResult.ErrorCEI = true;
                    objImportCeiResult.Message = "Ocorreu uma falha durante a integração ou seus ativos ainda não estão no CEI (B3). Por favor, tente novamente em alguns minutos.";
                }
            }
            else
            {
                objImportCeiResult.Imported = true;
            }

            objImportCeiResult.ListDividend = lstDividend;
            objImportCeiResult.ListStockOperation = lstStockOperation;
            objImportCeiResult.ListStockPortfolio = lstStockPortfolio;
            objImportCeiResult.ListTesouroDireto = lstTesouroDireto;
            objImportCeiResult.ListStockAveragePrice = lstStockAveragePrice;

            return objImportCeiResult;
        }

        private static void ConvertStockOperation(List<StockOperation> lstStockOperation, List<GenericImport> lstGenericNegotiation)
        {
            if (lstGenericNegotiation != null && lstGenericNegotiation.Count > 0)
            {
                lstGenericNegotiation.ForEach(genericOperation =>
                {
                    StockOperation stockNegotiation = new StockOperation();

                    decimal avgPrice = 0;

                    decimal.TryParse(genericOperation.AvgPrice, NumberStyles.Currency, new CultureInfo("pt-br"), out avgPrice);

                    stockNegotiation.Broker = GetBroker(genericOperation.Broker);
                    stockNegotiation.AveragePrice = avgPrice;
                    stockNegotiation.Symbol = RemoveFractionalLetter(genericOperation.Symbol);
                    stockNegotiation.NumberOfShares = Convert.ToInt64(genericOperation.NumShares.Replace(".", string.Empty).Replace(",", string.Empty));
                    stockNegotiation.OperationType = 1;
                    stockNegotiation.Market = !string.IsNullOrWhiteSpace(genericOperation.Market) ? genericOperation.Market.Trim() : null;
                    stockNegotiation.Expire = !string.IsNullOrWhiteSpace(genericOperation.Expire) ? genericOperation.Expire.Trim() : null;
                    stockNegotiation.Factor = !string.IsNullOrWhiteSpace(genericOperation.Factor) ? genericOperation.Factor.Trim() : null;
                    stockNegotiation.StockSpec = !string.IsNullOrWhiteSpace(genericOperation.StockSpec) ? genericOperation.StockSpec.Trim() : null;

                    if (!string.IsNullOrWhiteSpace(genericOperation.OperationType))
                    {
                        if (genericOperation.OperationType.ToLower().Trim() == "v")
                        {
                            stockNegotiation.OperationType = 2;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(genericOperation.Period))
                    {
                        DateTime eventDate;

                        if (DateTime.TryParse(genericOperation.Period, new CultureInfo("pt-br"), DateTimeStyles.None, out eventDate))
                        {
                            stockNegotiation.EventDate = eventDate;
                        }
                    }

                    lstStockOperation.Add(stockNegotiation);
                });
            }
        }

        private static string GetBroker(string broker)
        {
            if (!string.IsNullOrWhiteSpace(broker))
            {
                int startIndex = broker.IndexOf('-');

                if (broker.Length > startIndex + 1)
                {
                    broker = broker.Substring(startIndex + 1);
                }
            }

            return broker;
        }

        private string GetGeneric(ref List<GenericImport> lstGenericImport, string username, string password, string startDateFilter, CancellationTokenSource cancellationTokenSource = null)
        {
            string spiderName = "extract-all";

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.Culture = new System.Globalization.CultureInfo("pt-br");
            string json = CallSpider(_workingDirectoryCei, spiderName, username, password, startDateFilter, cancellationTokenSource);

            lstGenericImport = JsonConvert.DeserializeObject<List<GenericImport>>(json, jsSettings);

            return json;
        }

        private string CallSpider(string workingDirectory, string spiderName, string username, string password, string startDateFilter, CancellationTokenSource cancellationTokenSource = null)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = workingDirectory,
                    StandardOutputEncoding = Encoding.Default,
                    StandardInputEncoding = Encoding.Default,
                }
            };


            process.Start();
            // Pass multiple commands to cmd.exe
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    string scrapyCmd = string.Format(@"scrapy crawl {0} --nolog -o - -t json -a username={1} -a userpwd=", spiderName, username.Trim()) + password.Trim();

                    if (!string.IsNullOrWhiteSpace(startDateFilter))
                    {
                        scrapyCmd = string.Concat(scrapyCmd, " -a start_date_filter=", startDateFilter);
                    }

                    // Vital to activate Anaconda
                    sw.WriteLine(_workingDirectoryAnacondaActivate);
                    sw.WriteLine(scrapyCmd);
                }
            }


            var line = string.Empty;
            List<string> lstLines = new List<string>();
            StringBuilder sb = new StringBuilder();
            bool start = false;
            // read multiple output lines
            while (!process.StandardOutput.EndOfStream)
            {
                if (cancellationTokenSource != null && cancellationTokenSource.Token.IsCancellationRequested)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }

                line = process.StandardOutput.ReadLine();

                if (line.Equals("["))
                {
                    start = true;
                }

                if (start)
                {
                    sb.Append(line);
                }

                if (line.Equals("]"))
                {
                    start = false;
                }
                lstLines.Add(line);
            }

            string json = sb.ToString();

            return json;
        }

        public static string RemoveFractionalLetter(string stock)
        {
            if (!string.IsNullOrEmpty(stock) && (stock[stock.Length - 1].ToString().ToUpper() == "F"))
            {
                stock = stock.Remove(stock.Length - 1);
            }

            return stock;
        }

        private static bool CheckNewStocksToPortfolio(List<StockOperation> lstStockOperation)
        {
            bool exists = false;

            List<StockOperation> lstStockPortfolioGrouped = new List<StockOperation>();

            List<StockOperation> stocksOperationGpNew = lstStockOperation.Where(op => op.OperationType == 1 && !op.PriceAdjustNew && op.EventDate.HasValue && DateTime.Now.Subtract(op.EventDate.Value).TotalDays <= 6)
                                                                       .GroupBy(op => op.Symbol).Select(objStockOpGp => new StockOperation
                                                                       {
                                                                           Broker = objStockOpGp.First().Broker,
                                                                           Symbol = objStockOpGp.First().Symbol,
                                                                           NumberOfShares = objStockOpGp.Sum(c => c.NumberOfShares),
                                                                           AveragePrice = 0,
                                                                       }).ToList();

            if (stocksOperationGpNew != null && stocksOperationGpNew.Count > 0)
            {
                foreach (StockOperation stockOpNew in stocksOperationGpNew)
                {
                    if (lstStockPortfolioGrouped.Count == 0)
                    {
                        lstStockPortfolioGrouped.Add(stockOpNew);
                    }
                    else if (!lstStockPortfolioGrouped.Exists(op => op.Symbol == stockOpNew.Symbol))
                    {
                        lstStockPortfolioGrouped.Add(stockOpNew);
                    }
                }
            }

            if (lstStockPortfolioGrouped != null && lstStockPortfolioGrouped.Count > 0)
            {
                exists = true;
            }

            return exists;
        }


        public static void GetFakeResult(ref List<GenericImport> lstGenericImport)
        {
            StringBuilder sb = Scenario9();

            string json = sb.ToString();

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.Culture = new System.Globalization.CultureInfo("pt-br");
            lstGenericImport.AddRange(JsonConvert.DeserializeObject<List<GenericImport>>(json, jsSettings));
        }

        private static StringBuilder Scenario9()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"XPML11\", \"period\": \"18/10/2021\", \"numberOfShares\": \"10\", \"averagePrice\": \"100,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"XPML11\", \"paymentDate\": \"25/10/2021\", \"dividendType\": \"RENDIMENTO\", \"baseQuantity\": \"5\", \"grossValue\": \"100,56\", \"netValue\": \"100,56\", \"importType\": 1},");

            //sb.Append("{\"broker\": \"512 - TORO  CORRETORA - GRUPO XP\", \"symbol\": \"XPML11\", \"paymentDate\": \"25/10/2021\", \"dividendType\": \"RENDIMENTO\", \"baseQuantity\": \"5\", \"grossValue\": \"100,56\", \"netValue\": \"100,56\", \"importType\": 1},");

            sb.Append("]");
            return sb;
        }

        private static StringBuilder Scenario8()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            //sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BSLI4\", \"numberOfShares\": \"5\", \"importType\": 3},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BSLI4\", \"period\": \"05/09/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"20,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BSLI4\", \"period\": \"07/09/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"20,56\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BSLI4\", \"period\": \"05/09/2021\", \"numberOfShares\": \"4\", \"averagePrice\": \"20,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BSLI4\", \"period\": \"07/10/2021\", \"numberOfShares\": \"4\", \"averagePrice\": \"20,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BSLI4\", \"period\": \"08/10/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"20,56\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BSLI4\", \"period\": \"08/10/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"15,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");


            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ASIA11\", \"numberOfShares\": \"5\", \"importType\": 3},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ASIA11\", \"period\": \"05/09/2021\", \"numberOfShares\": \"3\", \"averagePrice\": \"10,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ASIA11\", \"period\": \"07/09/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"10,56\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ASIA11\", \"period\": \"08/10/2021\", \"numberOfShares\": \"3\", \"averagePrice\": \"10,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ASIA11\", \"period\": \"08/10/2021\", \"numberOfShares\": \"3\", \"averagePrice\": \"7,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");


            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"TECB11\", \"numberOfShares\": \"5\", \"importType\": 3},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"TECB11\", \"period\": \"08/09/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"15,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"TECB11\", \"period\": \"08/10/2021\", \"numberOfShares\": \"8\", \"averagePrice\": \"8,02\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");



            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"OIBR3\", \"period\": \"08/10/2021\", \"numberOfShares\": \"46\", \"averagePrice\": \"0,90\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("]");

            return sb;
        }

        private static StringBuilder Scenario7()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"OIBR3\", \"period\": \"21/06/2021\", \"numberOfShares\": \"1410\", \"averagePrice\": \"1,56\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"COGN3\", \"period\": \"21/06/2021\", \"numberOfShares\": \"1410\", \"averagePrice\": \"1,56\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");


            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BRBI11\", \"period\": \"21/06/2021\", \"numberOfShares\": \"1410\", \"averagePrice\": \"1,56\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"MXRF11\", \"paymentDate\": \"30/06/2021\", \"dividendType\": \"RENDIMENTO\", \"baseQuantity\": \"519,0001\", \"grossValue\": \"36,33\", \"netValue\": \"36,33\", \"importType\": 1},");

            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BBDC3\", \"paymentDate\": \"30/06/2021\", \"dividendType\": \"RENDIMENTO\", \"baseQuantity\": \"519,0001\", \"grossValue\": \"36,33\", \"netValue\": \"36,33\", \"importType\": 1},");

            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BBDC3\", \"period\": \"27/04/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"11,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");


            sb.Append("]");
            return sb;
        }

        private static StringBuilder Scenario6()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ITSA4\", \"period\": \"13/04/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"9,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ITSA4\", \"period\": \"13/04/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"9,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ITSA4\", \"period\": \"13/04/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"9,70\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"HBOR3\", \"period\": \"13/04/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"32,70\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");


            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"TORD11\", \"period\": \"27/04/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"11,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"ITSA4\", \"period\": \"27/04/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"11,70\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BBDC3\", \"period\": \"27/04/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"11,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");


            sb.Append("]");
            return sb;
        }

        private static StringBuilder Scenario5()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"numberOfShares\": \"145\", \"importType\": 3},");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"LIGT3\", \"numberOfShares\": \"1\", \"importType\": 3},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BRML3\", \"period\": \"06/04/2021\", \"numberOfShares\": \"300\", \"averagePrice\": \"9,98\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BRML3\", \"period\": \"06/04/2021\", \"numberOfShares\": \"2\", \"averagePrice\": \"9,99\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BBDC3\", \"period\": \"06/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"22,89\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"19\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"22,89\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,21\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,21\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"45\", \"averagePrice\": \"29,95\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,21\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"17\", \"averagePrice\": \"24,21\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"157\", \"averagePrice\": \"25,21\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("]");
            return sb;
        }


        private static StringBuilder Scenario4()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"numberOfShares\": \"145\", \"importType\": 3},");

            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"02/04/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"19\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC4\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"02/04/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"18,45\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC4\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"19\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"22,89\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,21\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,21\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"45\", \"averagePrice\": \"29,95\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,21\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"17\", \"averagePrice\": \"24,21\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"OIBR3\", \"period\": \"03/04/2021\", \"numberOfShares\": \"17\", \"averagePrice\": \"24,21\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"COGN3\", \"period\": \"02/04/2021\", \"numberOfShares\": \"69\", \"averagePrice\": \"3,77\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("]");
            return sb;
        }

        /// <summary>
        /// Buy/Sell
        /// </summary>
        /// <returns></returns>
        private static StringBuilder Scenario3()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"CASH3\", \"numberOfShares\": \"150\", \"importType\": 3},");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"BBDC3\", \"numberOfShares\": \"150\", \"importType\": 3},");


            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"11/01/2021\", \"numberOfShares\": \"18\", \"averagePrice\": \"21,65\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"13/01/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"26,24\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/01/2021\", \"numberOfShares\": \"23\", \"averagePrice\": \"26\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"22/01/2021\", \"numberOfShares\": \"24\", \"averagePrice\": \"30,85\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"22/01/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"30,84\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"26/01/2021\", \"numberOfShares\": \"10\", \"averagePrice\": \"28,46\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"35\", \"averagePrice\": \"35,35\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"27\", \"averagePrice\": \"35,25\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"35,25\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"3\", \"averagePrice\": \"35,28\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"08/02/2021\", \"numberOfShares\": \"20\", \"averagePrice\": \"33,95\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"09/02/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"30,76\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"28,50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"28,49\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"13\", \"averagePrice\": \"28,51\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"4\", \"averagePrice\": \"27,86\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"36\", \"averagePrice\": \"27,96\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"8\", \"averagePrice\": \"27,58\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"10\", \"averagePrice\": \"27,58\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"21\", \"averagePrice\": \"25,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"25,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,70\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"25,43\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"CASH3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");


            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"11/01/2021\", \"numberOfShares\": \"18\", \"averagePrice\": \"21,65\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"13/01/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"26,24\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/01/2021\", \"numberOfShares\": \"23\", \"averagePrice\": \"26\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"22/01/2021\", \"numberOfShares\": \"24\", \"averagePrice\": \"30,85\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"22/01/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"30,84\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"26/01/2021\", \"numberOfShares\": \"10\", \"averagePrice\": \"28,46\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"35\", \"averagePrice\": \"35,35\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"27\", \"averagePrice\": \"35,25\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"5\", \"averagePrice\": \"35,25\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"04/02/2021\", \"numberOfShares\": \"3\", \"averagePrice\": \"35,28\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"08/02/2021\", \"numberOfShares\": \"20\", \"averagePrice\": \"33,95\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"09/02/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"30,76\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"28,50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"1\", \"averagePrice\": \"28,49\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"13\", \"averagePrice\": \"28,51\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"4\", \"averagePrice\": \"27,86\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"12/02/2021\", \"numberOfShares\": \"36\", \"averagePrice\": \"27,96\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"8\", \"averagePrice\": \"27,58\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"10\", \"averagePrice\": \"27,58\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"21\", \"averagePrice\": \"25,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"25,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,70\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"21,70\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"17/03/2021\", \"numberOfShares\": \"50\", \"averagePrice\": \"25,43\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"BBDC3F\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("]");
            return sb;
        }

        /// <summary>
        /// Calc Avg Price
        /// </summary>
        /// <returns></returns>
        private static StringBuilder Scenario2()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"RECT11\", \"numberOfShares\": \"200\", \"importType\": 3},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"200\", \"averagePrice\": \"50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"55\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"19/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"25\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"20/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"55\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"21/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"10/03/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            //sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"22/02/2021\", \"numberOfShares\": \"200\", \"averagePrice\": \"55\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("]");
            return sb;
        }

        /// <summary>
        /// Rent Stocks
        /// </summary>
        /// <returns></returns>
        private static StringBuilder Scenario1()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"MGLU3\", \"numberOfShares\": \"179\", \"importType\": 3, \"gridType\": \"CARTEIRA LIVRE\"},");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"VALE3\", \"numberOfShares\": \"547\", \"importType\": 3, \"gridType\": \"CARTEIRA LIVRE\"},");

            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"MGLU3\", \"numberOfShares\": \"3750\", \"importType\": 3, \"gridType\": \"EMPRESTIMO DE ATIVOS\"},");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"VALE3\", \"numberOfShares\": \"700\", \"importType\": 3, \"gridType\": \"EMPRESTIMO DE ATIVOS\"},");


            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"MGLU3\", \"numberOfShares\": \"1.000\", \"importType\": 4, \"expire\": \"18/04/2021\"}, ");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"MGLU3\", \"numberOfShares\": \"1.000\", \"importType\": 4, \"expire\": \"18/04/2021\"}, ");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"VALE3\", \"numberOfShares\": \"700\", \"importType\": 4, \"expire\": \"18/04/2021\"}, ");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"VALE3\", \"numberOfShares\": \"700\", \"importType\": 4, \"expire\": \"18/04/2021\"}, ");
            sb.Append("{\"broker\": \"308-CLEAR CORRETORA - GRUPO XP\", \"symbol\": \"KLBN11\", \"numberOfShares\": \"1500\", \"importType\": 4, \"expire\": \"05/04/2021\"}, ");



            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"200\", \"averagePrice\": \"50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"18/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"55\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"19/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"25\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"20/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"55\", \"operationType\": \"V\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"21/02/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");
            sb.Append("{\"broker\": \"308 - CLEAR CORRETORA - GRUPO XP\", \"period\": \"10/03/2021\", \"numberOfShares\": \"100\", \"averagePrice\": \"50\", \"operationType\": \"C\", \"market\": \"Mercado a Vista\", \"expire\": \"\", \"symbol\": \"RECT11\", \"stockSpec\": \"FII REC RENDCI\", \"factor\": \"1\", \"importType\": 2},");

            sb.Append("]");
            return sb;
        }

    }
}
