// DOM Factory - Simple DOM manipulation utility
(function() {
    'use strict';
    
    window.DOMFactory = {
        // Create element with attributes
        create: function(tag, attributes, children) {
            var element = document.createElement(tag);
            
            if (attributes) {
                for (var key in attributes) {
                    if (attributes.hasOwnProperty(key)) {
                        element.setAttribute(key, attributes[key]);
                    }
                }
            }
            
            if (children) {
                if (Array.isArray(children)) {
                    children.forEach(function(child) {
                        element.appendChild(child);
                    });
                } else {
                    element.appendChild(children);
                }
            }
            
            return element;
        },
        
        // Find elements by selector
        find: function(selector, context) {
            context = context || document;
            return context.querySelector(selector);
        },
        
        // Find all elements by selector
        findAll: function(selector, context) {
            context = context || document;
            return context.querySelectorAll(selector);
        },
        
        // Add event listener
        on: function(element, event, handler) {
            if (element.addEventListener) {
                element.addEventListener(event, handler, false);
            } else if (element.attachEvent) {
                element.attachEvent('on' + event, handler);
            }
        },
        
        // Remove event listener
        off: function(element, event, handler) {
            if (element.removeEventListener) {
                element.removeEventListener(event, handler, false);
            } else if (element.detachEvent) {
                element.detachEvent('on' + event, handler);
            }
        }
    };
})();
