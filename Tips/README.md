# Tips

A simple tool that allows to show random tips to player when a level is loading.
Tips are exported as AssetBundles.

## Usage

1. Modify the content of the **tips** file (the language code is here only for future support of language but is not used yet)
2. Set this file to be in an asset bundle
3. Put the TipsLoader component in a game object of your scene, setting the asset bundle name and asset name according to those you entered just before
4. Display tips by putting the DisplayTips in a Canvas and assigning a TextMeshProUGUI component

![Tips in loading scene](Documentation~/images/async-loading.jpg)