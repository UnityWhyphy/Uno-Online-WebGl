mergeInto(LibraryManager.library, {
PerformActionWithCallback: function (gameObjectName, callbackFunctionName, message) {
        const unityMessage = Pointer_stringify(message); // Convert Unity string to JS string
        console.log("Message from Unity: " + unityMessage);

        // Simulate an asynchronous operation (e.g., an API call or timeout)
        setTimeout(() => {
            // Send a callback message to Unity
            unityInstance.SendMessage(Pointer_stringify(gameObjectName), Pointer_stringify(callbackFunctionName), "Callback response from JavaScript");
        }, 1000);
    }
});
