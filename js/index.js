let isRunning = true;
const isMobile = /iPhone|iPad|iPod|Android|webOS/i.test(navigator.userAgent);
const unityConfig = {
	dataUrl: "Build/Guidance_Web.data.unityweb",
	frameworkUrl: "Build/Guidance_Web.framework.js.unityweb",
	codeUrl: "Build/Guidance_Web.wasm.unityweb",
	streamingAssetsUrl: "StreamingAssets",
	companyName: "Lybell",
	productName: "Guidance",
	productVersion: "1.0.2",
	// matchWebGLToCanvasSize: false, // Uncomment this to separately control WebGL canvas render size and DOM element size.
	// devicePixelRatio: 1, // Uncomment this to override low DPI rendering on high DPI displays.
};
const loadingBarContainer = document.getElementById("unity-loading-container");
const loadingBarProgress = document.querySelector(".unity-progress-bar-full");

function setPauseGame(isPause)
{
	if(isPause)
	{
		isRunning = false;
		window.dispatchEvent(new FocusEvent("blur"));
	}
	else
	{
		isRunning = true;
		window.dispatchEvent(new FocusEvent("focus"));
	}
}

function loading(progress)
{
	loadingBarProgress.style.setProperty("--progress", progress * 100);
}

function mountOrientationEvent()
{
	const rotateIndicator = document.getElementById("mobile-horiz-indicator");
	function handleOrientationChange(isLandscape)
	{
		setPauseGame(!isLandscape);
		if(isLandscape) rotateIndicator.classList.add("hidden");
		else rotateIndicator.classList.remove("hidden");
	}

	if(screen.orientation != null)
	{
		screen.orientation.addEventListener("change", (e)=>{
			handleOrientationChange(e.target.type.startsWith("landscape"));
		});
		handleOrientationChange(screen.orientation.type.startsWith("landscape"));
	}
	else
	{
		const mediaQuery = window.matchMedia("(orientation: landscape)");
		mediaQuery.addListener(handleOrientationChange);
		handleOrientationChange(mediaQuery.matches);
	}
}

function mountEvent(unityInstance)
{
	loadingBarContainer.classList.add("hidden");
	const wrapper = document.querySelector(".unity-canvas-wrapper");
	const fullscreenButton = document.getElementById("fullscreen-button");
	fullscreenButton.addEventListener("click", ()=>{
		wrapper.requestFullscreen();
		fullscreenButton.classList.add("hidden");
	});
	wrapper.addEventListener("fullscreenchange", ()=>{
		if(!document.fullscreenElement) fullscreenButton.classList.remove("hidden");
	});
	if(isMobile) mountOrientationEvent();
}

function preventFocusWhenPaused(e)
{
	if(!isRunning) e.stopImmediatePropagation();
}

window.addEventListener("focus", preventFocusWhenPaused);
createUnityInstance(document.querySelector("#unity-canvas"), unityConfig, loading).then(mountEvent);
