namespace OrderIngest.Services.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAlertApiClient
{
    /// <summary>
    /// Posts an order alert message to the Alert API.
    /// </summary>
    /// <param name="message">The order alert message to post.</param>
    /// <returns>The completed <see cref="Task"/>.</returns>
    Task PublishAlertAsync<T>(T message);
}
