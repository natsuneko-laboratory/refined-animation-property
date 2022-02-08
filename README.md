# RefinedAnimationProperty

Unity Editor Extension that refine animation property editor.

## Features

Once installed, this extension will rewrite the behaviour around the Animation Property Editor as follows:

- Rewrites to the Popup Window does not close by itself when adding animation properties
- Rewrites to be possible to search by key name when adding animation properties
- Rewrites to the ID generation algorithm to make it less likely to be duplicated
- Supports to apply [easing functions](https://easings.net/) between the previous or next and current keyframes.

## Note

This extension used Harmony library, which may cause DLL conflicts with VRCSDK3 WORLD or UdonSharp.
In this case, please remove the included Harmony.dll and replace the reference in the Assembly Definition Files with that of VRCSDK3 WORLD or UdonSharp.

## Requirements

- Unity 2019.4.31f1

## Installation

Download UnityPackage from [Natsuneko Laboratory](https://natsuneko.moe)

## How to use

None, this editor extension is automatically enabled/worked

## License

This software is licensed under the License Zero Parity 7.0.0 and MIT license with exception License Zero Patron 1.0.0.
This is the same as the license for Husky 5.0, and can be summarized as follows:

- If you are using this software for open-source projects, you may use it under the terms of the MIT License.
- If you are using this software for commercial projects, you may use it under the terms of the License Zero Parity 7.0.0.
- If you are using this software for commercial projects, but you are supporting this project via GitHub Sponsors, Patreon, and others (monthly or yearly donations are supported), you may use it under the terms of the License Zero Patron 1.0.0.
- If you are contributing to this project, you SHOULD compliant with the MIT License.
