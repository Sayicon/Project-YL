/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   env_utils.c                                        :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/15 10:55:50 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/17 02:09:48 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

char	*get_env_value(t_env *env, const char *key)
{
	while (env)
	{
		if (ft_strcmp(env->key, key) == 0)
			return (env->value);
		env = env->next;
	}
	return (NULL);
}

char	*get_env_path(t_env *env)
{
	char	*path;

	path = get_env_value(env, "PATH");
	return (path);
}

void	env_update(t_env **env_head, char *key, char *value)
{
	t_env	*curr;

	curr = *env_head;
	while (curr)
	{
		if (ft_strcmp(curr->key, key) == 0)
		{
			free(curr->value);
			curr->value = ft_strdup(value);
			return ;
		}
		curr = curr->next;
	}
	env_createapnd(env_head, key, value, curr);
}

void	env_createapnd(t_env **head, char *key, char *val, t_env *curr)
{
	t_env	*node;

	node = env_new(key, val);
	if (!node)
		exit_perror("env_new failed");
	if (*head == NULL)
		*head = node;
	else
	{
		curr = *head;
		while (curr->next)
			curr = curr->next;
		curr->next = node;
	}
}
