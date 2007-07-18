using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenDental;
using OpenDentServer;

namespace OpenDentBusiness.Tests {
	class ServiceWrapper : MarshalByRefObject {
		public ServiceWrapper() {
			RemotingClient.OpenDentBusinessIsLocal = true;
			service = new OpenDentalService();
			thread = new Thread((ThreadStart)delegate() {
				service.ServiceWorkerMethod();
			});
		}

		private OpenDentalService service;
		public OpenDentalService Service {
			get { return service; }
		}

		private Thread thread;
		public Thread Thread {
			get { return thread; }
		}

		public void Start() {
			if (Thread.IsAlive)
				Stop();

			Thread.Start();
		}

		public void Stop() {
			if (Thread.IsAlive)
				Thread.Abort();
		}
	}
}
