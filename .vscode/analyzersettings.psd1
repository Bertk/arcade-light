AnalyzerSettings.psd1
@{
    Severity=@('Error','Warning')
    ExcludeRules=@('PSReviewUnusedParameter', 'PSUseDeclaredVarsMoreThanAssignments', 'PSAvoidUsingWriteHost', 'PSAvoidGlobalVars', 'PSAvoidAssignmentToAutomaticVariable', 'PSUseApprovedVerbs', 'PSUseSingularNouns', 'PSUseShouldProcessForStateChangingFunctions')
}