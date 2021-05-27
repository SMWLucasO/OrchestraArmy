
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;
using UnityEngine.Events;

namespace MidiPlayerTK
{
    /// <summary>
    /// MPTK Demo component able to load a Midi file from your list of Midi file or from a folder and displaying the list of events.
    /// </summary>
    public class TestMidiFileLoad : MonoBehaviour
    {
        /// <summary>
        /// </summary>

        // Manage skin
        public CustomStyle myStyle;

        // This PreFab must be present in your scene.
        public MidiFileLoader MidiLoader;

        public int MidiIndex = 0;
        public long StartTicks = 0;
        public long EndTicks = 0;
        public int PageToDisplay = 0;
        public string PathMidiFile;

        private Vector2 scrollerWindow = Vector2.zero;
        private int buttonWidth = 250;
        private PopupListItem PopMidi;

        private List<string> infoEvents;
        private Vector2 scrollPos = Vector2.zero;
        private GUIStyle butCentered;
        private GUIStyle labCentered;

        const int MAXLINEPAGE = 100;

        private void Awake()
        {
            MidiPlayerGlobal.LoadCurrentSF();
        }

        private void Start()
        {
            if (MidiLoader == null)
            {
                Debug.LogError("TestMidiFileLoad: there is no MidiFileLoader Prefab set in Inspector.");
            }

            PopMidi = new PopupListItem()
            {
                Title = "Select A Midi File",
                OnSelect = MidiChanged,
                Tag = "NEWMIDI",
                ColCount = 3,
                ColWidth = 250,
            };
            MidiChanged(null, MidiIndex, 0);
        }

        private void MidiChanged(object tag, int midiindex, int indexList)
        {
            Debug.Log("MidiChanged " + midiindex);
            MidiIndex = midiindex;
            MidiLoader.MPTK_MidiIndex = midiindex;
            MidiLoader.MPTK_Load();
            StartTicks = 0;
            EndTicks = MidiLoader.MPTK_TickLast;
            PageToDisplay = 0;
            scrollPos = new Vector2(0, 0);
            infoEvents = new List<string>();
        }

        void OnGUI()
        {
            if (!HelperDemo.CheckSFExists()) return;

            // Set custom Style. Good for background color 3E619800
            if (myStyle == null)
                myStyle = new CustomStyle();

            if (butCentered == null)
            {
                butCentered = new GUIStyle("Button");
                butCentered.alignment = TextAnchor.MiddleCenter;
                butCentered.fontSize = 16;
            }

            if (labCentered == null)
            {
                labCentered = new GUIStyle("Label");
                labCentered.alignment = TextAnchor.MiddleCenter;
                labCentered.fontSize = 16;
            }

            if (MidiLoader != null)
            {
                scrollerWindow = GUILayout.BeginScrollView(scrollerWindow, false, false, GUILayout.Width(Screen.width));

                // Display popup in first to avoid activate other layout behind
                PopMidi.Draw(MidiPlayerGlobal.MPTK_ListMidi, MidiIndex, myStyle);

                MainMenu.Display("Test Midi File Loader - Demonstrate how to use the MPTK API to load a Midi file", myStyle);

                //
                // Left column: Midi action and info
                // ---------------------------------

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(myStyle.BacgDemos, GUILayout.Width(450));

                GUILayout.BeginHorizontal();
                GUILayout.Label("Select and load a Midi File from the internal MidiDB:", GUILayout.Width(350));
                // Open the popup to select a midi
                if (GUILayout.Button("Open Popup", GUILayout.Width(120)))
                    PopMidi.Show = !PopMidi.Show;
                PopMidi.Position(ref scrollerWindow);
                GUILayout.EndHorizontal();
#if MPTK_PRO
                // Select a midi from a local file on the desktop
                GUILayout.Label("Or other option, from a local Midi file. Enter a full path:", GUILayout.Width(350));
                GUILayout.BeginHorizontal();
                PathMidiFile = GUILayout.TextField(PathMidiFile, GUILayout.Width(350));
                if (GUILayout.Button(new GUIContent("Load Midi File", ""), GUILayout.Width(120)))
                {
                    if (MidiLoader.MPTK_Load(PathMidiFile))
                    {
                        StartTicks = 0;
                        EndTicks = MidiLoader.MPTK_TickLast;
                        PageToDisplay = 0;
                        scrollPos = new Vector2(0, 0);
                        infoEvents = new List<string>();
                    }
                }
                GUILayout.EndHorizontal();

#endif
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button($"Read Midi Events from ticks {StartTicks} to ticks {EndTicks}", GUILayout.Height(40)))
                {
                    infoEvents = new List<string>();
                    List<MPTKEvent> mptkEvents = MidiLoader.MPTK_ReadMidiEvents(StartTicks, EndTicks);
                    foreach (MPTKEvent mptkEvent in mptkEvents)
                    {
                        infoEvents.Add(mptkEvent.ToString());
                    }
                }
                GUILayout.EndHorizontal();

                //string midiname = "no midi defined";
                //if (MidiIndex >= 0 && MidiPlayerGlobal.MPTK_ListMidi != null && MidiIndex < MidiPlayerGlobal.MPTK_ListMidi.Count)
                //    midiname = MidiPlayerGlobal.MPTK_ListMidi[MidiIndex].Label;
                GUILayout.Label("Current Midi file: " + MidiLoader.MPTK_MidiName, myStyle.TitleLabel3);

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Read Midi Events from: " + StartTicks, myStyle.TitleLabel3, GUILayout.Width(220));
                StartTicks = (long)GUILayout.HorizontalSlider((float)StartTicks, 0f, (float)MidiLoader.MPTK_TickLast, GUILayout.Width(buttonWidth));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Read Midi Events to: " + EndTicks, myStyle.TitleLabel3, GUILayout.Width(220));
                EndTicks = (long)GUILayout.HorizontalSlider((float)EndTicks, 0f, (float)MidiLoader.MPTK_TickLast, GUILayout.Width(buttonWidth));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Duration in seconds: ", myStyle.TitleLabel3, GUILayout.Width(220));
                GUILayout.Label(MidiLoader.MPTK_Duration.TotalSeconds.ToString(), myStyle.TitleLabel3);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Beat per Measure: ", myStyle.TitleLabel3, GUILayout.Width(220));
                GUILayout.Label(MidiLoader.MPTK_NumberBeatsMeasure.ToString());
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Quarter per Beat: ", myStyle.TitleLabel3, GUILayout.Width(220));
                GUILayout.Label(MidiLoader.MPTK_NumberQuarterBeat.ToString(), myStyle.TitleLabel3);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("MPTK_InitialTempo: ", myStyle.TitleLabel3, GUILayout.Width(220));
                GUILayout.Label(Convert.ToInt32(MidiLoader.MPTK_InitialTempo).ToString(), myStyle.TitleLabel3);
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                GUILayout.Label("This class can be used only to load a Midi file and read events. There is no Midi sequencer and no Midi Synthesizer. Rather, used the prefab MidiFilePlayer to play a Midi file.", myStyle.TitleLabel3);

                // End left column
                GUILayout.EndVertical();

                if (infoEvents != null && infoEvents.Count > 0)
                {
                    GUILayout.BeginVertical(myStyle.BacgDemos, GUILayout.Width(650));

                    //
                    // Right Column: midi infomation, lyrics, ...
                    // ------------------------------------------
                    GUILayout.BeginHorizontal(myStyle.BacgDemos);

                    if (GUILayout.Button("<<", butCentered, GUILayout.Height(40))) PageToDisplay = 0;
                    if (GUILayout.Button("<", butCentered, GUILayout.Height(40))) PageToDisplay--;
                    GUILayout.Label("page " + (PageToDisplay + 1).ToString() + " / " + (infoEvents.Count / MAXLINEPAGE + 1).ToString(), labCentered, GUILayout.Width(150), GUILayout.Height(40));
                    if (GUILayout.Button(">", butCentered, GUILayout.Height(40))) PageToDisplay++;
                    if (GUILayout.Button(">>", butCentered, GUILayout.Height(40))) PageToDisplay = infoEvents.Count / MAXLINEPAGE;

                    GUILayout.EndHorizontal();

                    if (PageToDisplay < 0) PageToDisplay = 0;
                    if (PageToDisplay * MAXLINEPAGE > infoEvents.Count) PageToDisplay = infoEvents.Count / MAXLINEPAGE;

                    string infoToDisplay = "";
                    for (int i = PageToDisplay * MAXLINEPAGE; i < (PageToDisplay + 1) * MAXLINEPAGE; i++)
                        if (i < infoEvents.Count)
                            infoToDisplay += infoEvents[i] + "\n";

                    GUILayout.BeginHorizontal();

                    scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);//, GUILayout.Height(heightLyrics));
                    GUILayout.Label(infoToDisplay, myStyle.TextFieldMultiLine);
                    GUILayout.EndScrollView();

                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                GUILayout.EndScrollView();

            }
        }
    }
}