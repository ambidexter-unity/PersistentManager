using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.GameService;
using UnityEngine;
using UnityEngine.Assertions;

namespace Common.PersistentManager
{
	public abstract class PersistentManagerBase : MonoBehaviour, IPersistentManager
	{
		private enum StorageType
		{
			All,
			File,
			PlayerPrefs
		}

		public const string PersistentKey = "good_night_tamafish";
		public static string SavedDataPath => Path.Combine(Application.persistentDataPath, "savedata.json");

		private bool _ready = false;

		// Вспомогательные классы для сериализации записей сохраняемыж данных.
		[Serializable]
		private class RawDataRecord
		{
			// ReSharper disable InconsistentNaming
			public string key;

			public string value;
			// ReSharper restore InconsistentNaming
		}

		[Serializable]
		private class RawData
		{
			// ReSharper disable InconsistentNaming
			public List<RawDataRecord> rawData = new List<RawDataRecord>();
			// ReSharper restore InconsistentNaming
		}
		//---------------------------------------//

		private readonly RawData _rawPlayerPrefsData = new RawData();
		private readonly RawData _rawFileData = new RawData();
		private bool _isValid;

		// IGameService

		public void Initialize(params object[] args)
		{
			RestoreGameData();
			_ready = true;
		}

		
		public bool IsReady => _ready;

		// \IGameService		

		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus) return;
			PersistGameData(StorageType.All);
		}

		// IPersistentManager

		public void Persist<T>(T data, bool asPlayerPrefs = false, bool lazy = false) where T : IPersistent<T>, new()
		{
			Assert.IsTrue(IsReady);

			if (asPlayerPrefs)
			{
				_rawPlayerPrefsData.rawData.RemoveAll(record => record.key == data.PersistentId);
				_rawPlayerPrefsData.rawData.Add(new RawDataRecord
				{
					key = data.PersistentId,
					value = JsonUtility.ToJson(data)
				});
			}
			else
			{
				_rawFileData.rawData.RemoveAll(record => record.key == data.PersistentId);
				_rawFileData.rawData.Add(new RawDataRecord {key = data.PersistentId, value = JsonUtility.ToJson(data)});
			}

			_isValid = false;
			if (!lazy) PersistGameData(asPlayerPrefs ? StorageType.PlayerPrefs : StorageType.File);
		}

		public bool Restore<T>(T data) where T : IPersistent<T>, new()
		{
			Assert.IsTrue(IsReady);

			var rawData = _rawFileData.rawData.SingleOrDefault(record => record.key == data.PersistentId) ??
			              _rawPlayerPrefsData.rawData.SingleOrDefault(record => record.key == data.PersistentId);
			if (rawData == null)
			{
				return false;
			}

			data.Restore(JsonUtility.FromJson<T>(rawData.value));
			return true;
		}

		public T GetPersistentValue<T>() where T : IPersistent<T>, new()
		{
			Assert.IsTrue(IsReady);

			var res = new T();
			Restore(res);
			return res;
		}

		public bool Remove<T>(T data) where T : IPersistent<T>, new()
		{
			Assert.IsTrue(IsReady);

			if (data == null)
			{
				return false;
			}

			return Remove(data.PersistentId);
		}

		public bool Remove(string id)
		{
			Assert.IsTrue(IsReady);

			var res = false;
			if (_rawFileData.rawData.RemoveAll(record => record.key == id) > 0)
			{
				_isValid = false;
				PersistGameData(StorageType.File);
				res = true;
			}

			if (_rawPlayerPrefsData.rawData.RemoveAll(record => record.key == id) > 0)
			{
				_isValid = false;
				PersistGameData(StorageType.PlayerPrefs);
				res = true;
			}

			return res;
		}

		// \IPersistentManager

		private void RestoreGameData()
		{
			var path = SavedDataPath;
			if (File.Exists(path))
			{
				JsonUtility.FromJsonOverwrite(File.ReadAllText(path), _rawFileData);
			}
			else
			{
				_rawFileData.rawData.Clear();
			}
			
			if (PlayerPrefs.HasKey(PersistentKey))
			{
				JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(PersistentKey), _rawPlayerPrefsData);
			}
			else
			{
				_rawPlayerPrefsData.rawData.Clear();
			}

			_isValid = true;
		}

		private void PersistGameData(StorageType storageType)
		{
			if (_isValid) return;

			if (storageType == StorageType.PlayerPrefs || storageType == StorageType.All)
			{
				PlayerPrefs.SetString(PersistentKey, JsonUtility.ToJson(_rawPlayerPrefsData));
				PlayerPrefs.Save();
			}

			if (storageType == StorageType.File || storageType == StorageType.All)
			{
				File.WriteAllText(SavedDataPath, JsonUtility.ToJson(_rawFileData));
			}

			_isValid = true;
		}


		
		public event GameServiceReadyHandler ReadyEvent;
	}
}
