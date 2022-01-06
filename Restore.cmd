@echo off
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0eng\commonlight\Build.ps1""" -restore %*"