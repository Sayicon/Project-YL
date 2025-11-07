/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   builtin_export.c                                   :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/23 15:27:40 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/23 18:32:44 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int		handle_export_arg(char *arg, t_env **env, char *name, char *value);
size_t	find_index(char *arg, char c);
void	split_name_value(char *arg, size_t eq, char **name, char **value);
int		is_valid_identifier(char *name);

int	builtin_export(char **args, t_env **env)
{
	int		status;
	int		i;
	size_t	eq;
	char	*name;
	char	*value;

	status = 0;
	if (!args[1])
	{
		print_exported(*env);
		return (0);
	}
	i = 1;
	while (args[i])
	{
		eq = find_index(args[i], '=');
		split_name_value(args[i], eq, &name, &value);
		if (handle_export_arg(args[i], env, name, value))
			status = 1;
		free(name);
		if (value)
			free(value);
		i++;
	}
	return (status);
}

// Finds the index of character c in the string arg.
// 1. Returns position of first occurence of c.
// 2. Returns length of arg if c not found.
size_t	find_index(char *arg, char c)
{
	size_t	i;

	i = 0;
	while (arg[i] && arg[i] != c)
		i++;
	return (i);
}

// Handles a single export argument.
// 1. Checks if name is a valid identifier.
// 2. Prints error and returns 1 if valid.
// 3. Updates environment with name and value if valid.
// 4. Adds name to export list if value is NULL.
// Returns 0 on success, 1 on error.
int	handle_export_arg(char *arg, t_env **env, char *name, char *value)
{
	int		err;

	err = 0;
	if (!is_valid_identifier(name))
	{
		ft_putstr_fd("minishell: export: `", STDERR_FILENO);
		ft_putstr_fd(arg, STDERR_FILENO);
		ft_putstr_fd("': not a valid identifier\n", STDERR_FILENO);
		err = 1;
	}
	else if (value)
		env_update(env, name, value);
	else
		env_export_only(env, name);
	return (err);
}

// Splits argument into name and value parts.
// 1. If '=' not found, name = whole arg, value = NULL.
// 2. Otherwise, name = substring before '=', value = after '='.
void	split_name_value(char *arg, size_t eq, char **name, char **value)
{
	size_t	len;

	len = ft_strlen(arg);
	if (eq == len)
	{
		*name = ft_strdup(arg);
		*value = NULL;
	}
	else
	{
		*name = ft_substr(arg, 0, eq);
		*value = ft_substr(arg, eq + 1, len - eq - 1);
	}
}

// Validates if name is a valid environment variable identifier.
// 1. Must not be NULL or empty.
// 2. First character must be a letter or underscore.
// 3. Remaining characters must be alphanumeric or underscore.
// Returns 1 if valid, 0 otherwise.
int	is_valid_identifier(char *name)
{
	int	i;

	if (!name)
		return (0);
	if (name[0] == '\0')
		return (0);
	if (!ft_isalpha(name[0]) && name[0] != '_')
		return (0);
	i = 1;
	while (name[i])
	{
		if (!ft_isalnum(name[i]) && name[i] != '_')
			return (0);
		i++;
	}
	return (1);
}
