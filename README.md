# Unity Dialog Editor

This is a custom editor for creating simple dialog trees within Unity. The dialog tree editor is integrated with my localization editor, which can be found [here](https://github.com/ballen1/UnityLocalizationEditor).

To access the dialog tree editor, navigate to the menu:

![Alt text](Dialog1.PNG?raw=true "Access the editor here")

The dialog tree editor allows you to choose the language file that you want to work with for creating your dialogs.

A search bar allows the developer to search for a string and provides an option to copy the string key (see the localization manual) to clipboard so that she can use the key to create a dialog node or dialog option.

There are two views: node view and tree view.

Node view allows the developer to create named nodes which are associated with text. These nodes will later be used to construct dialog trees. This is the node view:

![Alt text](Dialog2.PNG?raw=true "Node View")

Tree view allows the developer to create the dialog interactions. The developer can select a node and create dialog options for that node. The dialog options allow the developer to specify a string and what the next node should be if the user selects that option. The tree view looks like this:

![Alt text](Dialog3.PNG?raw=true "Tree View")
