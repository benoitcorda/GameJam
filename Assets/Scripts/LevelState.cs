public class LevelState {

	// The possible level states. These must match up with the 
	// scene numbering in the build settings.
	public enum State : int {
		Chatty = 3,
		Clingy = 4,
		Sequence = 2,
	};

	// The current level state; that is, which level to go to next.
	public static State levelState = LevelState.State.Chatty;

	// The intro level sequence that depends on the state.
	public static State levelSequence = LevelState.State.Sequence;
}
