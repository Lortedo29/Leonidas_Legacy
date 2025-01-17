* #### Custom Editor Window for MyBox settings with logo and stuff ❤

* #### Allow Commentary to be a type (to be shown and edited inside of other inspector) +   
  * Statically assign via script
  * Make readonly
  
* #### AnimationStateReference not working with nested inspector

* #### Setting to get MyBoxUpdate warnings about new versions + about bugfixes

* #### OnPlaymodeLogger (MyLogger?) to accumulate log messages before playmode and log on playmode
  * Some systems log things on BeforePlaymode and such messages will be erased if ClearOnPlay is set on Console
  * RequireLayerOtRagAttributeHandler and some other system...
 
* #### RangedFloat/Int and MinMaxFloat/Int functionality should be merged?

* #### Test coverage, man (ง •̀_•́)ง

* #### Update GUIDComponent? Remove it? 

* #### IPrepareAlways to use Prepare() without bool return

* #### Auto generate MyLayers and MyTags scripts with const strings/ints representing actual, well, layers and tags

* #### Allow to use every feature separately, without MyBox

* #### Add documentation links for MonoBehaviour types

* #### Allow to opt out MustBeAssigned and AutoProperty checks? Add highlight in inspector if disibled?

* #### MustBeAssigned may work with ScriptableObjects in project? 
  * Allow to disable this feature? Measure performance
  
* ### DisplayInspector — Allow to fold inspector
  
* #### Multiscene asset
  * To save/load opened loaded/active scenes in editor
  * Write last opened asset id to EditorPrefs, bind to Save event to update asset automatically?
  
* #### MonoBehaviourPool
  * Static class with GetPoolable<MB>(this GameObject prefab) and DeactivatePoolable<MB>(this MB behaviour) or something

* #### MyGizmos => Arrow, Dotted, Cross?
  * MyGizmos, MyHandles, MyDebug should be reconsidered
  
* #### MyGizmosHandler && MyOnGUIHandler
  * MyDebug.DrawText is working only in OnDrawGizmos :( 
  * I want to access OnDrawGizmos in non-MonoBehaviour scripts
  * MyOnGUI may be useful, for instance, in FPSCounter feature
  * MB with static access, with lazy initialization and HideAndDontSave?
    * EveryFrame subscription is heavy...
    * Push struct with IDraw and logic to draw with gizmos, remove pushed structs from MyGizmosHandler.OnDrawGizmos? Measure performance
    * Some way to draw every-frame Gizmos with system, that run only once per x seconds?
		
* #### Highlight empty fields with MustBeAssignedAttribute in inspector
  * Same with AutoProperty fields, if none found on GO	
	
* #### AssetPresetPreprocessor is very slow on matching assets, profile & optimize
  * At least skip import if nothing changed?
  
* #### Node editor tools (ง ͠° ͟ل͜ ͡°)ง
	
* #### AnimationCreator is pretty cool with simple Idle-Play or looping Idle animators. 
  * So the idea is to add Context menu item to generate AnimationController asset with imbedded AnimationClips assets
  * The simplest case is Animator with Play clip and empty default Idle. To play single animation on some event
  * Another case is Animator winh one Default looping clip fir infinite cycled animation
  * Is it possible to play animation without animator? 
    * https://github.com/Unity-Technologies/SimpleAnimation
	
* #### EmbeddedAnimationCreator to EmbeddedAssetCreator? 
  * It is possible to embed one asset inside of another via script. Will be cool to have such functionality via ContextMenuItems
  * It's cool to have, to pack related assets into one parent asset. Like animation clips in animator or relates SO assets or whatewer
  * How to unparent assets?
	
* #### MyBundleUtility is a mess. Might be useful
  * Yeah well it looks like Unity now have its own Bandles Tools
  * Tools to build bundles out of scenes (with multiscene solutions)
    * And handle bundles loading/unloading on scene load/unload?
		
* #### SingleScriptableObject. No more messy Create/ with CreateAssetMenuAttribute for settings SO's
  * Can it cache itself somehow...? To gain static or single entry point access
  * Base class (attribute?) to ensure that there is at least one SO of this type
  * SelectFolder/Filename prompt will automatically appear on recompilation if no instances of SO found? 
    * Warning if there is more than one?
    * Static accessor? Or probably better to add Load<T> method?

* #### TemplatesCreator
  * Template is custom code snippets, like "Create/C# Script" but for custom things
  * Add a way to add templates as separate assets
  * There is no way to have MenuItems with runtime naming :(?
    * MenuItems is separate feature/wrapper for TemplateCreator?
    * WHOA! Figured it out. I may generate separate script with MenuItems in any selected by used folder. Find this script and get its path to regenerate if needed!
   
* #### Conditionally remove some features like extension/hotkeys with Conditional Compilation?

* #### Somehow apply Custom Drawer to empty array? ConditionalField is need this
  * Custom MyArray type :)? Well it's terrible idea I guess
  * Maybe new unity UI Elements will change it somehow

* #### ConditionalField should hide Separator attribute?
