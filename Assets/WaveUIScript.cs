using TowerDefense.Level;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI.HUD
{
	/// <summary>
	/// A class for displaying the wave feedback
	/// </summary>
	public class WaveUIScript : MonoBehaviour
	{
		/// <summary>
		/// The text element to display information on
		/// </summary>
		public Text display;
		public Spawner spawner;
		public GameObject wave;


		public Image waveFillImage;



		/// <summary>
		/// cache the total amount of waves
		/// Update the display 
		/// and Subscribe to waveChanged
		/// </summary>
		protected virtual void Start()
		{
		}

		/// <summary>
		/// Write the current wave amount to the display
		/// </summary>
		protected void UpdateDisplay()
		{
			int currentWave = spawner.startingWave;
			string output = string.Format("{0}", currentWave);
			display.text = output;
		}

		protected virtual void Update()
		{
			waveFillImage.fillAmount = (float)spawner.enemiesSpawned / spawner.GetWaveSize();
			UpdateDisplay();
			wave.SetActive(spawner.isRunning);
		}


	}
}