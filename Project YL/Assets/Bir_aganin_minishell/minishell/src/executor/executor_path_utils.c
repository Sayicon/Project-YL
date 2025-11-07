/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   executor_path_utils.c                              :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/18 23:00:32 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/16 00:24:13 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int	is_executable_command(char *cmd, t_env *env)
{
	struct stat	sb;

	if (is_explicit_executable(cmd))
	{
		if (access(cmd, F_OK) != 0)
			return (0);
		if (stat(cmd, &sb) == 0 && S_ISDIR(sb.st_mode))
			return (0);
		if (access(cmd, X_OK) == 0)
			return (1);
		return (-1);
	}
	return (search_in_path(cmd, env));
}

int	is_explicit_executable(char *cmd)
{
	if (ft_strchr(cmd, '/') == NULL)
		return (0);
	return (1);
}

int	search_in_path(char *cmd, t_env *env)
{
	char	**dirs;
	char	*full;
	int		i;

	dirs = ft_split(get_env_path(env), ':');
	if (!dirs)
		return (0);
	i = 0;
	while (dirs[i])
	{
		full = ft_strjoin3(dirs[i], "/", cmd);
		if (!full)
			break ;
		if (access(full, X_OK) == 0)
		{
			free(full);
			free_string_array(dirs);
			return (1);
		}
		free(full);
		i++;
	}
	free_string_array(dirs);
	return (0);
}
