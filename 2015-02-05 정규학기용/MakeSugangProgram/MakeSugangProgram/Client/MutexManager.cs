using System.Threading;

namespace MakeSugangProgram.Client
{
	/// <summary>
	/// Description of MutexManager.
	/// </summary>
	internal static class MutexManager
	{
		private static bool boolNew;
		private static string mutexName = "Sugang";
		private static Mutex mutex;
		
		internal static bool IsExistMutex()
		{
			CreateMutex();
			
			if(CheckOwnership())
			{
				return false;
			}
			
			return true;
		}
		
		private static void CreateMutex()
		{
			mutex = new Mutex(true, mutexName, out boolNew);
		}
		
		private static bool CheckOwnership()
		{
			if(boolNew)
				return true;
			return false;
		}
	}
}
