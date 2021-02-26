var gulp = require('gulp'),
    sass = require('gulp-sass'),
    myth = require('gulp-myth'),
    csso = require('gulp-csso'),
    uglify = require('gulp-uglifyjs'),
    concat = require('gulp-concat'),
    browserSync = require('browser-sync').create();

gulp.task('styles', function(done) {
  gulp.src(['./client/src/sass/*.sass', './client/src/sass/*.scss', '!./client/src/sass/_*.sass'])
    .pipe(sass())
    .pipe(myth())
    .pipe(concat('styles.css'))
    .pipe(gulp.dest('./client/dist/css'))
    .pipe(browserSync.reload({stream:true}));
  done();
});

gulp.task('js', function(done) {
  gulp.src(['./client/src/js/*.js'])
    .pipe(concat('app_bundle.js'))
    .pipe(gulp.dest('./client/dist/js'))
    .pipe(browserSync.reload({stream:true}));
  done();
});

gulp.task('server', function(done) {
  browserSync.init({
    server: './client/dist'
  });

  gulp.watch('./client/src/sass/*.sass', gulp.series('styles'));
  gulp.watch('./client/src/js/*.js', gulp.series('js'));
  gulp.watch('./client/src/*.html').on('change', () => {
    browserSync.reload();
    done();
  });
});

gulp.task('default', gulp.series('styles', 'js', 'server'));