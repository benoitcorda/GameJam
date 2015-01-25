public class LevelState {

	// The possible level states. These must match up with the 
	// scene numbering in the build settings.
	public enum State : int {
		Chatty = 2,
		Clingy,
	};

	// The current level state; that is, which level to go to next.
	public static State levelState = 0;
}
