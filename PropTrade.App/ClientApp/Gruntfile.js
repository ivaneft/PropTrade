module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({

        pkg: grunt.file.readJSON('package.json'),

        karma: {
            unit: {
                options: {
                    basePath: '',
                    frameworks: ['jasmine'],
                    files: [
                        'bower_components/angular/angular.js',
                        'bower_components/angular-route/angular-route.js',
                        'bower_components/angular-mocks/angular-mocks.js',
                        'js/**/*.js',
                        'test/**/*Spec.js'
                    ],
                    port: 9876,
                    colors: true,
                    logLevel: 'INFO',
                    autoWatch: true,
                    browsers: ['PhantomJS'],
                    singleRun: true
                }
            }
        }       

    });

    // Load the needed plugins 
    grunt.loadNpmTasks('grunt-karma');

    // Default task(s).
    grunt.registerTask('default', ['karma']);
};