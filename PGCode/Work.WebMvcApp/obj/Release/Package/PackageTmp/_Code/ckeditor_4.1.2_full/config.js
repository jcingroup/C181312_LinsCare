/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    config.uiColor = '#AFD6FF';
    config.language = 'zh';
    config.enterMode = CKEDITOR.ENTER_BR;
    
    config.stylesSet=
    [
     { name: '大標題', element: 'h3' },
     { name: '小標題', element: 'h4' },
     { name: '段落', element: 'p' }
    ];
    
    config.contentsCss = ['../../Content/css/editor.css'];
};
