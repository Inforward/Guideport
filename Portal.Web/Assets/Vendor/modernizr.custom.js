/*! Modernizr 3.0.0-beta (Custom Build) | MIT
 *  Build: http://modernizr.com/download/#-canvas-csstransforms-csstransitions-fontface-localstorage-touchevents-userselect-cssclasses-dontmin
 */
; (function (window, document, undefined) {

    var classes = [];


    var tests = [];


    var ModernizrProto = {
        // The current version, dummy
        _version: 'v3.0.0pre',

        // Any settings that don't work as separate modules
        // can go in here as configuration.
        _config: {
            classPrefix: '',
            enableClasses: true,
            usePrefixes: true
        },

        // Queue of tests
        _q: [],

        // Stub these for people who are listening
        on: function (test, cb) {
            // I don't really think people should do this, but we can
            // safe guard it a bit.
            // -- NOTE:: this gets WAY overridden in src/addTest for
            // actual async tests. This is in case people listen to
            // synchronous tests. I would leave it out, but the code
            // to *disallow* sync tests in the real version of this
            // function is actually larger than this.
            setTimeout(function () {
                cb(this[test]);
            }, 0);
        },

        addTest: function (name, fn, options) {
            tests.push({ name: name, fn: fn, options: options });
        },

        addAsyncTest: function (fn) {
            tests.push({ name: null, fn: fn });
        }
    };



    // Fake some of Object.create
    // so we can force non test results
    // to be non "own" properties.
    var Modernizr = function () { };
    Modernizr.prototype = ModernizrProto;

    // Leak modernizr globally when you `require` it
    // rather than force it here.
    // Overwrite name so constructor name is nicer :D
    Modernizr = new Modernizr();


    /*!
    {
      "name": "Local Storage",
      "property": "localstorage",
      "caniuse": "namevalue-storage",
      "tags": ["storage"],
      "knownBugs": [],
      "notes": [],
      "warnings": [],
      "polyfills": [
        "joshuabell-polyfill",
        "cupcake",
        "storagepolyfill",
        "amplifyjs",
        "yui-cacheoffline",
        "textstorage"
      ]
    }
    !*/

    // In FF4, if disabled, window.localStorage should === null.

    // Normally, we could not test that directly and need to do a
    //   `('localStorage' in window) && ` test first because otherwise Firefox will
    //   throw bugzil.la/365772 if cookies are disabled

    // Also in iOS5 Private Browsing mode, attempting to use localStorage.setItem
    // will throw the exception:
    //   QUOTA_EXCEEDED_ERRROR DOM Exception 22.
    // Peculiarly, getItem and removeItem calls do not throw.

    // Because we are forced to try/catch this, we'll go aggressive.

    // Just FWIW: IE8 Compat mode supports these features completely:
    //   www.quirksmode.org/dom/html5.html
    // But IE8 doesn't support either with local files

    Modernizr.addTest('localstorage', function () {
        var mod = 'modernizr';
        try {
            localStorage.setItem(mod, mod);
            localStorage.removeItem(mod);
            return true;
        } catch (e) {
            return false;
        }
    });


    /**
     * is returns a boolean for if typeof obj is exactly type.
     */
    function is(obj, type) {
        return typeof obj === type;
    }
    ;

    // Run through all tests and detect their support in the current UA.
    function testRunner() {
        var featureNames;
        var feature;
        var aliasIdx;
        var result;
        var nameIdx;
        var featureName;
        var featureNameSplit;

        for (var featureIdx in tests) {
            featureNames = [];
            feature = tests[featureIdx];
            // run the test, throw the return value into the Modernizr,
            //   then based on that boolean, define an appropriate className
            //   and push it into an array of classes we'll join later.
            //
            //   If there is no name, it's an 'async' test that is run,
            //   but not directly added to the object. That should
            //   be done with a post-run addTest call.
            if (feature.name) {
                featureNames.push(feature.name.toLowerCase());

                if (feature.options && feature.options.aliases && feature.options.aliases.length) {
                    // Add all the aliases into the names list
                    for (aliasIdx = 0; aliasIdx < feature.options.aliases.length; aliasIdx++) {
                        featureNames.push(feature.options.aliases[aliasIdx].toLowerCase());
                    }
                }
            }

            // Run the test, or use the raw value if it's not a function
            result = is(feature.fn, 'function') ? feature.fn() : feature.fn;


            // Set each of the names on the Modernizr object
            for (nameIdx = 0; nameIdx < featureNames.length; nameIdx++) {
                featureName = featureNames[nameIdx];
                // Support dot properties as sub tests. We don't do checking to make sure
                // that the implied parent tests have been added. You must call them in
                // order (either in the test, or make the parent test a dependency).
                //
                // Cap it to TWO to make the logic simple and because who needs that kind of subtesting
                // hashtag famous last words
                featureNameSplit = featureName.split('.');

                if (featureNameSplit.length === 1) {
                    Modernizr[featureNameSplit[0]] = result;
                }
                else if (featureNameSplit.length === 2) {
                    // cast to a Boolean, if not one already
                    /* jshint -W053 */
                    if (Modernizr[featureNameSplit[0]] && !(Modernizr[featureNameSplit[0]] instanceof Boolean)) {
                        Modernizr[featureNameSplit[0]] = new Boolean(Modernizr[featureNameSplit[0]]);
                    }

                    Modernizr[featureNameSplit[0]][featureNameSplit[1]] = result;
                }

                classes.push((result ? '' : 'no-') + featureNameSplit.join('-'));
            }
        }
    }

    ;

    var docElement = document.documentElement;


    // Pass in an and array of class names, e.g.:
    //  ['no-webp', 'borderradius', ...]
    function setClasses(classes) {
        var className = docElement.className;
        var classPrefix = Modernizr._config.classPrefix || '';

        // Change `no-js` to `js` (we do this regardles of the `enableClasses`
        // option)
        // Handle classPrefix on this too
        var reJS = new RegExp('(^|\\s)' + classPrefix + 'no-js(\\s|$)');
        className = className.replace(reJS, '$1' + classPrefix + 'js$2');

        if (Modernizr._config.enableClasses) {
            // Add the new classes
            className += ' ' + classPrefix + classes.join(' ' + classPrefix);
            docElement.className = className;
        }

    }

    ;

    var createElement = function () {
        return document.createElement.apply(document, arguments);
    };

    /*!
    {
      "name": "Canvas",
      "property": "canvas",
      "caniuse": "canvas",
      "tags": ["canvas", "graphics"],
      "polyfills": ["flashcanvas", "excanvas", "slcanvas", "fxcanvas"]
    }
    !*/
    /* DOC
    Detects support for the `<canvas>` element for 2D drawing.
    */

    // On the S60 and BB Storm, getContext exists, but always returns undefined
    // so we actually have to call getContext() to verify
    // github.com/Modernizr/Modernizr/issues/issue/97/
    Modernizr.addTest('canvas', function () {
        var elem = createElement('canvas');
        return !!(elem.getContext && elem.getContext('2d'));
    });


    // List of property values to set for css tests. See ticket #21
    var prefixes = (ModernizrProto._config.usePrefixes ? ' -webkit- -moz- -o- -ms- '.split(' ') : []);

    // expose these for the plugin API. Look in the source for how to join() them against your input
    ModernizrProto._prefixes = prefixes;



    function getBody() {
        // After page load injecting a fake body doesn't work so check if body exists
        var body = document.body;

        if (!body) {
            // Can't use the real body create a fake one.
            body = createElement('body');
            body.fake = true;
        }

        return body;
    }

    ;

    // Inject element with style element and some CSS rules
    function injectElementWithStyles(rule, callback, nodes, testnames) {
        var mod = 'modernizr';
        var style;
        var ret;
        var node;
        var docOverflow;
        var div = createElement('div');
        var body = getBody();

        if (parseInt(nodes, 10)) {
            // In order not to give false positives we create a node for each test
            // This also allows the method to scale for unspecified uses
            while (nodes--) {
                node = createElement('div');
                node.id = testnames ? testnames[nodes] : mod + (nodes + 1);
                div.appendChild(node);
            }
        }

        // <style> elements in IE6-9 are considered 'NoScope' elements and therefore will be removed
        // when injected with innerHTML. To get around this you need to prepend the 'NoScope' element
        // with a 'scoped' element, in our case the soft-hyphen entity as it won't mess with our measurements.
        // msdn.microsoft.com/en-us/library/ms533897%28VS.85%29.aspx
        // Documents served as xml will throw if using ­ so use xml friendly encoded version. See issue #277
        style = ['­', '<style id="s', mod, '">', rule, '</style>'].join('');
        div.id = mod;
        // IE6 will false positive on some tests due to the style element inside the test div somehow interfering offsetHeight, so insert it into body or fakebody.
        // Opera will act all quirky when injecting elements in documentElement when page is served as xml, needs fakebody too. #270
        (!body.fake ? div : body).innerHTML += style;
        body.appendChild(div);
        if (body.fake) {
            //avoid crashing IE8, if background image is used
            body.style.background = '';
            //Safari 5.13/5.1.4 OSX stops loading if ::-webkit-scrollbar is used and scrollbars are visible
            body.style.overflow = 'hidden';
            docOverflow = docElement.style.overflow;
            docElement.style.overflow = 'hidden';
            docElement.appendChild(body);
        }

        ret = callback(div, rule);
        // If this is done after page load we don't want to remove the body so check if body exists
        if (body.fake) {
            body.parentNode.removeChild(body);
            docElement.style.overflow = docOverflow;
            // Trigger layout so kinetic scrolling isn't disabled in iOS6+
            docElement.offsetHeight;
        } else {
            div.parentNode.removeChild(div);
        }

        return !!ret;

    }

    ;

    var testStyles = ModernizrProto.testStyles = injectElementWithStyles;

    /*!
    {
      "name": "@font-face",
      "property": "fontface",
      "authors": ["Diego Perini", "Mat Marquis"],
      "tags": ["css"],
      "knownBugs": [
        "False Positive: WebOS http://github.com/Modernizr/Modernizr/issues/342",
        "False Postive: WP7 http://github.com/Modernizr/Modernizr/issues/538"
      ],
      "notes": [{
        "name": "@font-face detection routine by Diego Perini",
        "href": "http://javascript.nwbox.com/CSSSupport/"
      },{
        "name": "Filament Group @font-face compatibility research",
        "href": "https://docs.google.com/presentation/d/1n4NyG4uPRjAA8zn_pSQ_Ket0RhcWC6QlZ6LMjKeECo0/edit#slide=id.p"
      },{
        "name": "Filament Grunticon/@font-face device testing results",
        "href": "https://docs.google.com/spreadsheet/ccc?key=0Ag5_yGvxpINRdHFYeUJPNnZMWUZKR2ItMEpRTXZPdUE#gid=0"
      },{
        "name": "CSS fonts on Android",
        "href": "http://stackoverflow.com/questions/3200069/css-fonts-on-android"
      },{
        "name": "@font-face and Android",
        "href": "http://archivist.incutio.com/viewlist/css-discuss/115960"
      }]
    }
    !*/

    var blacklist = (function () {
        var ua = navigator.userAgent;
        var wkvers = ua.match(/applewebkit\/([0-9]+)/gi) && parseFloat(RegExp.$1);
        var webos = ua.match(/w(eb)?osbrowser/gi);
        var wppre8 = ua.match(/windows phone/gi) && ua.match(/iemobile\/([0-9])+/gi) && parseFloat(RegExp.$1) >= 9;
        var oldandroid = wkvers < 533 && ua.match(/android/gi);
        return webos || oldandroid || wppre8;
    }());
    if (blacklist) {
        Modernizr.addTest('fontface', false);
    } else {
        testStyles('@font-face {font-family:"font";src:url("https://")}', function (node, rule) {
            var style = document.getElementById('smodernizr');
            var sheet = style.sheet || style.styleSheet;
            var cssText = sheet ? (sheet.cssRules && sheet.cssRules[0] ? sheet.cssRules[0].cssText : sheet.cssText || '') : '';
            var bool = /src/i.test(cssText) && cssText.indexOf(rule.split(' ')[0]) === 0;
            Modernizr.addTest('fontface', bool);
        });
    }
    ;
    /*!
    {
      "name": "Touch Events",
      "property": "touchevents",
      "caniuse" : "touch",
      "tags": ["media", "attribute"],
      "notes": [{
        "name": "Touch Events spec",
        "href": "http://www.w3.org/TR/2013/WD-touch-events-20130124/"
      }],
      "warnings": [
        "Indicates if the browser supports the Touch Events spec, and does not necessarily reflect a touchscreen device"
      ],
      "knownBugs": [
        "False-positive on some configurations of Nokia N900",
        "False-positive on some BlackBerry 6.0 builds – https://github.com/Modernizr/Modernizr/issues/372#issuecomment-3112695"
      ]
    }
    !*/
    /* DOC
    Indicates if the browser supports the W3C Touch Events API.
    
    This *does not* necessarily reflect a touchscreen device:
    
    * Older touchscreen devices only emulate mouse events
    * Modern IE touch devices implement the Pointer Events API instead: use `Modernizr.pointerevents` to detect support for that
    * Some browsers & OS setups may enable touch APIs when no touchscreen is connected
    * Future browsers may implement other event models for touch interactions
    
    See this article: [You Can't Detect A Touchscreen](http://www.stucox.com/blog/you-cant-detect-a-touchscreen/).
    
    It's recommended to bind both mouse and touch/pointer events simultaneously – see [this HTML5 Rocks tutorial](http://www.html5rocks.com/en/mobile/touchandmouse/).
    
    This test will also return `true` for Firefox 4 Multitouch support.
    */

    // Chrome (desktop) used to lie about its support on this, but that has since been rectified: http://crbug.com/36415
    Modernizr.addTest('touchevents', function () {
        var bool;
        if (('ontouchstart' in window) || (window.DocumentTouch && document instanceof DocumentTouch) || !!window.navigator.msMaxTouchPoints) {
            bool = true;
        } else {
            var query = ['@media (', prefixes.join('touch-enabled),('), 'heartz', ')', '{#modernizr{top:9px;position:absolute}}'].join('');
            testStyles(query, function (node) {
                bool = node.offsetTop === 9;
            });
        }
        return bool;
    });


    // Following spec is to expose vendor-specific style properties as:
    //   elem.style.WebkitBorderRadius
    // and the following would be incorrect:
    //   elem.style.webkitBorderRadius

    // Webkit ghosts their properties in lowercase but Opera & Moz do not.
    // Microsoft uses a lowercase `ms` instead of the correct `Ms` in IE8+
    //   erik.eae.net/archives/2008/03/10/21.48.10/

    // More here: github.com/Modernizr/Modernizr/issues/issue/21
    var omPrefixes = 'Webkit Moz O ms';


    var cssomPrefixes = (ModernizrProto._config.usePrefixes ? omPrefixes.split(' ') : []);
    ModernizrProto._cssomPrefixes = cssomPrefixes;


    var domPrefixes = (ModernizrProto._config.usePrefixes ? omPrefixes.toLowerCase().split(' ') : []);
    ModernizrProto._domPrefixes = domPrefixes;


    /**
     * contains returns a boolean for if substr is found within str.
     */
    function contains(str, substr) {
        return !!~('' + str).indexOf(substr);
    }

    ;

    // Change the function's scope.
    function fnBind(fn, that) {
        return function () {
            return fn.apply(that, arguments);
        };
    }

    ;

    /**
     * testDOMProps is a generic DOM property test; if a browser supports
     *   a certain property, it won't return undefined for it.
     */
    function testDOMProps(props, obj, elem) {
        var item;

        for (var i in props) {
            if (props[i] in obj) {

                // return the property name as a string
                if (elem === false) return props[i];

                item = obj[props[i]];

                // let's bind a function
                if (is(item, 'function')) {
                    // bind to obj unless overriden
                    return fnBind(item, elem || obj);
                }

                // return the unbound function or obj or value
                return item;
            }
        }
        return false;
    }

    ;

    /**
     * Create our "modernizr" element that we do most feature tests on.
     */
    var modElem = {
        elem: createElement('modernizr')
    };

    // Clean up this element
    Modernizr._q.push(function () {
        delete modElem.elem;
    });



    var mStyle = {
        style: modElem.elem.style
    };

    // kill ref for gc, must happen before
    // mod.elem is removed, so we unshift on to
    // the front of the queue.
    Modernizr._q.unshift(function () {
        delete mStyle.style;
    });



    // Helper function for converting camelCase to kebab-case,
    // e.g. boxSizing -> box-sizing
    function domToCSS(name) {
        return name.replace(/([A-Z])/g, function (str, m1) {
            return '-' + m1.toLowerCase();
        }).replace(/^ms-/, '-ms-');
    }
    ;

    // Function to allow us to use native feature detection functionality if available.
    // Accepts a list of property names and a single value
    // Returns `undefined` if native detection not available
    function nativeTestProps(props, value) {
        var i = props.length;
        // Start with the JS API: http://www.w3.org/TR/css3-conditional/#the-css-interface
        if ('CSS' in window && 'supports' in window.CSS) {
            // Try every prefixed variant of the property
            while (i--) {
                if (window.CSS.supports(domToCSS(props[i]), value)) {
                    return true;
                }
            }
            return false;
        }
            // Otherwise fall back to at-rule (for FF 17 and Opera 12.x)
        else if ('CSSSupportsRule' in window) {
            // Build a condition string for every prefixed variant
            var conditionText = [];
            while (i--) {
                conditionText.push('(' + domToCSS(props[i]) + ':' + value + ')');
            }
            conditionText = conditionText.join(' or ');
            return injectElementWithStyles('@supports (' + conditionText + ') { #modernizr { position: absolute; } }', function (node) {
                return (window.getComputedStyle ?
                        getComputedStyle(node, null) :
                        node.currentStyle)['position'] == 'absolute';
            });
        }
        return undefined;
    }
    ;

    // testProps is a generic CSS / DOM property test.

    // In testing support for a given CSS property, it's legit to test:
    //    `elem.style[styleName] !== undefined`
    // If the property is supported it will return an empty string,
    // if unsupported it will return undefined.

    // We'll take advantage of this quick test and skip setting a style
    // on our modernizr element, but instead just testing undefined vs
    // empty string.

    // Because the testing of the CSS property names (with "-", as
    // opposed to the camelCase DOM properties) is non-portable and
    // non-standard but works in WebKit and IE (but not Gecko or Opera),
    // we explicitly reject properties with dashes so that authors
    // developing in WebKit or IE first don't end up with
    // browser-specific content by accident.

    function testProps(props, prefixed, value, skipValueTest) {
        skipValueTest = is(skipValueTest, 'undefined') ? false : skipValueTest;

        // Try native detect first
        if (!is(value, 'undefined')) {
            var result = nativeTestProps(props, value);
            if (!is(result, 'undefined')) {
                return result;
            }
        }

        // Otherwise do it properly
        var afterInit, i, prop, before;

        // If we don't have a style element, that means
        // we're running async or after the core tests,
        // so we'll need to create our own elements to use
        if (!mStyle.style) {
            afterInit = true;
            mStyle.modElem = createElement('modernizr');
            mStyle.style = mStyle.modElem.style;
        }

        // Delete the objects if we
        // we created them.
        function cleanElems() {
            if (afterInit) {
                delete mStyle.style;
                delete mStyle.modElem;
            }
        }

        for (i in props) {
            prop = props[i];
            before = mStyle.style[prop];

            if (!contains(prop, '-') && mStyle.style[prop] !== undefined) {

                // If value to test has been passed in, do a set-and-check test.
                // 0 (integer) is a valid property value, so check that `value` isn't
                // undefined, rather than just checking it's truthy.
                if (!skipValueTest && !is(value, 'undefined')) {

                    // Needs a try catch block because of old IE. This is slow, but will
                    // be avoided in most cases because `skipValueTest` will be used.
                    try {
                        mStyle.style[prop] = value;
                    } catch (e) { }

                    // If the property value has changed, we assume the value used is
                    // supported. If `value` is empty string, it'll fail here (because
                    // it hasn't changed), which matches how browsers have implemented
                    // CSS.supports()
                    if (mStyle.style[prop] != before) {
                        cleanElems();
                        return prefixed == 'pfx' ? prop : true;
                    }
                }
                    // Otherwise just return true, or the property name if this is a
                    // `prefixed()` call
                else {
                    cleanElems();
                    return prefixed == 'pfx' ? prop : true;
                }
            }
        }
        cleanElems();
        return false;
    }

    ;

    /**
     * testPropsAll tests a list of DOM properties we want to check against.
     *     We specify literally ALL possible (known and/or likely) properties on
     *     the element including the non-vendor prefixed one, for forward-
     *     compatibility.
     */
    function testPropsAll(prop, prefixed, elem, value, skipValueTest) {

        var ucProp = prop.charAt(0).toUpperCase() + prop.slice(1),
        props = (prop + ' ' + cssomPrefixes.join(ucProp + ' ') + ucProp).split(' ');

        // did they call .prefixed('boxSizing') or are we just testing a prop?
        if (is(prefixed, 'string') || is(prefixed, 'undefined')) {
            return testProps(props, prefixed, value, skipValueTest);

            // otherwise, they called .prefixed('requestAnimationFrame', window[, elem])
        } else {
            props = (prop + ' ' + (domPrefixes).join(ucProp + ' ') + ucProp).split(' ');
            return testDOMProps(props, prefixed, elem);
        }
    }

    // Modernizr.testAllProps() investigates whether a given style property,
    //     or any of its vendor-prefixed variants, is recognized
    // Note that the property names must be provided in the camelCase variant.
    // Modernizr.testAllProps('boxSizing')
    ModernizrProto.testAllProps = testPropsAll;



    /**
     * testAllProps determines whether a given CSS property, in some prefixed
     * form, is supported by the browser. It can optionally be given a value; in
     * which case testAllProps will only return true if the browser supports that
     * value for the named property; this latter case will use native detection
     * (via window.CSS.supports) if available. A boolean can be passed as a 3rd
     * parameter to skip the value check when native detection isn't available,
     * to improve performance when simply testing for support of a property.
     *
     * @param prop - String naming the property to test
     * @param value - [optional] String of the value to test
     * @param skipValueTest - [optional] Whether to skip testing that the value
     *                        is supported when using non-native detection
     *                        (default: false)
     */
    function testAllProps(prop, value, skipValueTest) {
        return testPropsAll(prop, undefined, undefined, value, skipValueTest);
    }
    ModernizrProto.testAllProps = testAllProps;

    /*!
    {
      "name": "CSS Transforms",
      "property": "csstransforms",
      "caniuse": "transforms2d",
      "tags": ["css"]
    }
    !*/

    Modernizr.addTest('csstransforms', testAllProps('transform', 'scale(1)', true));

    /*!
    {
      "name": "CSS Transitions",
      "property": "csstransitions",
      "caniuse": "css-transitions",
      "tags": ["css"]
    }
    !*/

    Modernizr.addTest('csstransitions', testAllProps('transition', 'all', true));

    /*!
    {
      "name": "CSS user-select",
      "property": "userselect",
      "caniuse": "user-select-none",
      "authors": ["ryan seddon"],
      "tags": ["css"],
      "builderAliases": ["css_userselect"],
      "notes": [{
        "name": "Related Modernizr Issue",
        "href": "https://github.com/Modernizr/Modernizr/issues/250"
      }]
    }
    !*/

    //https://github.com/Modernizr/Modernizr/issues/250
    Modernizr.addTest('userselect', testAllProps('userSelect', 'none', true));


    // Run each test
    testRunner();

    // Remove the "no-js" class if it exists
    setClasses(classes);

    delete ModernizrProto.addTest;
    delete ModernizrProto.addAsyncTest;

    // Run the things that are supposed to run after the tests
    for (var i = 0; i < Modernizr._q.length; i++) {
        Modernizr._q[i]();
    }

    // Leak Modernizr namespace
    window.Modernizr = Modernizr;



})(this, document);