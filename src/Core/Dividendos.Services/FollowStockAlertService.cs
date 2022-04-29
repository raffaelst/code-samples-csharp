using Dividendos.Entity.Entities;
using Dividendos.Entity.Enum;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using K.Logger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class FollowStockAlertService : BaseService, IFollowStockAlertService
    {
        public FollowStockAlertService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void ApplyAlertsRules(IEnumerable<FollowStockView> followStockViews, INotificationService notificationService, IDeviceService deviceService, INotificationHistoricalService notificationHistoricalService, ICacheService cacheService, ILogger logger)
        {
            //Verifica o preço alvo do alerta e o tipo (abaixo ou acima de)

            foreach (FollowStockView item in followStockViews)
            {
                ResultServiceObject<IEnumerable<Device>> devices = new ResultServiceObject<IEnumerable<Device>>();
                string alertMessage = null;
                string alertTitle = "Alerta de preço alcançado";

                if (item.FollowStockTypeID == (int)FollowStockTypeEnum.HigherThan)
                {
                    if (item.MarketPrice > item.TargetPrice)
                    {
                        devices = deviceService.GetByUser(item.UserID);

                        if (string.IsNullOrWhiteSpace(item.CustomMessage))
                        {
                            alertMessage = string.Format("Seu alerta de preço ({0} - {1} com preço maior que {4} {2}) foi alcançado às {3}. Veja mais detalhes no App Dividendos.me.", item.Symbol, item.CompanyName, item.TargetPrice.ToString("n2", new CultureInfo("pt-br")), DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), item.IdCountry.Equals((int)CountryEnum.Brazil) ? "R$" : "$");
                        }
                        else
                        {
                            alertMessage = string.Format("Seu alerta de preço ({0}) referente a {1} foi alcançado às {2}. Veja mais detalhes no App Dividendos.me.", item.CustomMessage, item.Symbol, DateTime.Now.ToString("dd/MM/yy HH:mm:ss"));
                        }

                        //desativa o alerta
                        ResultServiceObject<FollowStockAlert> resultService = this.GetByGuid(item.FollowStockAlertGuid);
                        resultService.Value.Reached = true;
                        resultService.Value.ReachedDate = DateTime.Now;
                        this.Update(resultService.Value);

                        //colocar no histórico
                        notificationHistoricalService.New(alertTitle, alertMessage, item.UserID, AppScreenNameEnum.HomeFollow.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, cacheService);
                    }
                }
                else
                {
                    if (item.MarketPrice < item.TargetPrice)
                    {
                        devices = deviceService.GetByUser(item.UserID);

                        if (string.IsNullOrWhiteSpace(item.CustomMessage))
                        {
                            alertMessage = string.Format("Seu alerta de preço ({0} - {1} com preço menor que {4} {2}) foi alcançado às {3}. Veja mais detalhes no App Dividendos.me.", item.Symbol, item.CompanyName, item.TargetPrice.ToString("n2", new CultureInfo("pt-br")), DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), item.IdCountry.Equals((int)CountryEnum.Brazil) ? "R$" : "$");
                        }
                        else
                        {
                            alertMessage = string.Format("Seu alerta de preço ({0}) referente a {1} foi alcançado às {2}. Veja mais detalhes no App Dividendos.me.", item.CustomMessage, item.Symbol, DateTime.Now.ToString("dd/MM/yy HH:mm:ss"));
                        }

                        //desativa o alerta
                        ResultServiceObject<FollowStockAlert> resultService = this.GetByGuid(item.FollowStockAlertGuid);
                        resultService.Value.Reached = true;
                        resultService.Value.ReachedDate = DateTime.Now;
                        this.Update(resultService.Value);

                        //colocar no histórico
                        notificationHistoricalService.New(alertTitle, alertMessage, item.UserID, AppScreenNameEnum.HomeFollow.ToString(), PushRedirectTypeEnum.Internal.ToString(), null, cacheService);
                    }
                }

                if (devices.Value != null)
                {
                    foreach (var itemDevice in devices.Value)
                    {
                        try
                        {
                            notificationService.SendPush(alertTitle, alertMessage, itemDevice, new PushRedirect() { PushRedirectType = PushRedirectTypeEnum.Internal, AppScreenName = AppScreenNameEnum.HomeFollow });
                        }
                        catch (Exception ex)
                        {
                            _ = logger.SendErrorAsync(ex);
                        }
                    }
                }
            }
        }

        public ResultServiceObject<FollowStockAlert> Add(FollowStockAlert followStockAlert)
        {
            ResultServiceObject<FollowStockAlert> resultService = new ResultServiceObject<FollowStockAlert>();
            followStockAlert.CreatedDate = DateTime.Now;
            followStockAlert.FollowStockAlertGuid = Guid.NewGuid();
            followStockAlert.Active = true;
            followStockAlert.Reached = false;
            followStockAlert.FollowStockAlertID = _uow.FollowStockAlertRepository.Insert(followStockAlert);

            resultService.Value = followStockAlert;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<FollowStockView>> GetAllAlertsActiveAndNotReached()
        {
            IEnumerable<FollowStockView> followStocks = _uow.FollowStockViewRepository.GetAllAlertsActiveAndNotReached();

            ResultServiceObject<IEnumerable<FollowStockView>> resultService = new ResultServiceObject<IEnumerable<FollowStockView>>();

            resultService.Value = followStocks;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<FollowStockAlert>> GetAllActiveAlertsByFollowStock(Guid followStockGuid)
        {
            IEnumerable<FollowStockAlert> followStocks = _uow.FollowStockAlertRepository.GetAllActiveAlertsByFollowStock(followStockGuid.ToString());

            ResultServiceObject<IEnumerable<FollowStockAlert>> resultService = new ResultServiceObject<IEnumerable<FollowStockAlert>>();

            resultService.Value = followStocks;

            return resultService;
        }

        public ResultServiceObject<IEnumerable<FollowStockAlertView>> GetAllActiveAlertsViewByFollowStock(Guid followStockGuid)
        {
            IEnumerable<FollowStockAlertView> followStocks = _uow.FollowStockAlertViewRepository.GetAllActiveAlertsByFollowStock(followStockGuid.ToString());

            ResultServiceObject<IEnumerable<FollowStockAlertView>> resultService = new ResultServiceObject<IEnumerable<FollowStockAlertView>>();

            resultService.Value = followStocks;

            return resultService;
        }

        public ResultServiceObject<FollowStockAlert> Update(FollowStockAlert followStockAlert)
        {
            ResultServiceObject<FollowStockAlert> resultService = new ResultServiceObject<FollowStockAlert>();

            followStockAlert = _uow.FollowStockAlertRepository.Update(followStockAlert);

            resultService.Value = followStockAlert;

            return resultService;
        }

        public ResultServiceObject<FollowStockAlert> GetByGuid(Guid followStockAlertGuid)
        {
            ResultServiceObject<FollowStockAlert> resultService = new ResultServiceObject<FollowStockAlert>();

            FollowStockAlert followStockAlert = _uow.FollowStockAlertRepository.Select(item => item.FollowStockAlertGuid == followStockAlertGuid).FirstOrDefault();

            resultService.Value = followStockAlert;

            return resultService;
        }
    }
}
