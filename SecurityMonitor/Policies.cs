namespace SecurityMonitor
{
    public static class Policies
    {
        public static (bool, string) CheckOperation(string[] input)
        {
            switch (input[0])
            {
                case "input_command_handler":
                    switch (input[1])
                    {
                        case "пылесос":
                        case "жалюзи":
                            return (true, "non_critical_external_component_management_system");

                        case "климат-контроль":
                        case "свет":
                        case "script":
                        case "start":
                            return (true, "authorization_system");
                        default:
                            return (false, string.Empty);
                    }
                case "non_critical_data_analysis_system":
                    return (true, "non_critical_external_component_management_system");
                case "non_critical_external_component_management_system":
                    return (true, "non_critical_external_components");
                case "authorization_system":
                    return (true, "critical_input_command_processing_system");
                case "critical_input_command_processing_system":
                    if (input[1] != "script" && input[1] != "start")
                        return (true, "critical_external_component_management_system");
                    
                    return (true, "system_for_reading_and_writing_user_scripts");
                case "critical_external_component_management_system":
                    return (true, "critical_external_components");
                case "critical_data_analysis_system":
                    return (true, "critical_input_command_processing_system");
                case "system_for_reading_and_writing_user_scripts":
                    switch (input[1])
                    {
                        case "script":
                        case "start":
                            return (true, "сustom_script_library");
                        default:
                            return (true, "critical_external_component_management_system");
                    }
                case "сustom_script_library":
                    return (true, "system_for_reading_and_writing_user_scripts");
                default:
                    return (false, string.Empty);
            }
        }
    }
}
