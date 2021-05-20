using System;
using System.Collections;
using System.Threading.Tasks;
using FileStorage;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
	[Header("Buttons")]
	[SerializeField] private Button _uploadButton;
	[SerializeField] private Button _downloadButton;

	[Header("Text fields")] 
	[SerializeField] private Text _text;

	[Header("Addresses")]
	[SerializeField] private string _fsBaseAddress;
	[SerializeField] private string _productsBaseAddress;

	[Header("Task config")]
	[SerializeField] private int _bufferSize;
	[SerializeField] private int _taskCount;

	[Header("Build location config")]
	[SerializeField] private string _folderPath;
	[SerializeField] private string _startFileName;

	private string _currentMessage;

	private void Awake()
	{
		_uploadButton.onClick.AddListener(Upload);
	}

	private void Update()
	{
		_text.text =
			_currentMessage;
	}


	public void Upload()
	{
		using (FileStorageManager fsm =
			new FileStorageManager(_fsBaseAddress,
				_productsBaseAddress,
				Convert.ToInt32(_bufferSize),
				Convert.ToInt32(_taskCount)))
		{
			fsm.CurrentStateChanged +=
				s => _currentMessage = s;

			var md5 =
				StartCoroutine(fsm.UploadBuild(_folderPath, "04b79864-c61b-4540-9885-51a8bdd15552", _startFileName, 2)
					.AsEnumerator());
		}
	}
}

public static class TaskExtensions
{
	public static IEnumerator AsEnumerator(this Task task)
	{
		while (!task.IsCompleted)
		{
			yield return null;
		}

		yield return null;
	}
}
